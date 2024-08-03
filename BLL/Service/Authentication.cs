using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;
using DAL.Interface;
using BLL.Models;
using DAL.Data;

namespace BLL.Authentication
{
    public class AuthenticationService
    {
        private readonly IConfiguration _config;
        private readonly IusersRepository _userRepository;

        public AuthenticationService(IConfiguration config, IusersRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        private string GenerateJSONWebToken(string username, List<string> roles)
        {
            var secretKey = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("La clé secrète JWT n'est pas définie dans la configuration.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            claims.Add(new Claim("custom_info", "info"));

            var jwtIssuer = _config["Jwt:Issuer"];
            var jwtAudience = _config["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                throw new InvalidOperationException("L'émetteur ou le public JWT n'est pas défini dans la configuration.");
            }

            var token = new JwtSecurityToken(
                jwtIssuer,
                jwtAudience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password, string salt)
        {
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                password: Encoding.UTF8.GetBytes(password),
                salt: Encoding.UTF8.GetBytes(salt),
                iterations: 10000, // Recommandé pour une meilleure sécurité
                hashAlgorithm: HashAlgorithmName.SHA512,
                outputLength: 32);
            return Convert.ToHexString(hash);
        }

        public async Task RegisterUserAsync(string username, string password, string role)
        {
            var existingUser = await _userRepository.GetUsersByPseudo(username);

            if (existingUser != null)
            {
                throw new Exception("L'utilisateur existe déjà");
            }

            var salt = GenerateSalt();
            var passwordHash = HashPassword(password, salt);

            var newUser = new UsersData
            {
                Pseudo = username,
                Passwd = passwordHash,
                Salt = salt,
                Roles = role
            };

            await _userRepository.AddAsync(newUser);
        }

        private string GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var saltBytes = new byte[16];
                rng.GetBytes(saltBytes);
                return Convert.ToBase64String(saltBytes);
            }
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetUsersByPseudo(username)
                        ?? throw new Exception("Utilisateur ou mot de passe invalide");

            var passwordHash = HashPassword(password, user.Salt);
            if (user.Passwd != passwordHash)
            {
                throw new Exception("Utilisateur ou mot de passe incorrect");
            }

            var roles = new List<string> { user.Roles };

            var token = GenerateJSONWebToken(username, roles);
            return token;
        }
    }
}
