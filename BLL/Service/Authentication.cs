using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;
using DAL.Interface;
using DAL.Repository;
using BLL.Models;
using DAL.Data;
using BLL.Mapper;

namespace BLL.Authentication
{
    public class AuthenticationService
    {
        private readonly IConfiguration _config;
        private readonly IusersRepository _userRepository; // Assumez que vous avez un d�p�t pour les utilisateurs

        public AuthenticationService(IConfiguration config, IusersRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        private string GenerateJSONWebToken(string username, List<string> roles)
        {
            // R�cup�rer la cl� secr�te depuis la configuration
            var secretKey = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("La cl� secr�te JWT n'est pas d�finie dans la configuration.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Cr�er la liste des claims
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            // Ajouter les claims de r�le
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Ajouter d'autres claims si n�cessaire
            claims.Add(new Claim("custom_info", "info"));

            var jwtIssuer = _config["Jwt:Issuer"];
            var jwtAudience = _config["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                throw new InvalidOperationException("L'�metteur ou le public JWT n'est pas d�fini dans la configuration.");
            }

            var token = new JwtSecurityToken(
                jwtIssuer,
                jwtAudience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(120), // Utilisez UTC pour �viter les probl�mes de fuseau horaire
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password, string salt)
        {
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                password: Encoding.UTF8.GetBytes(password),
                salt: Encoding.UTF8.GetBytes(salt),
                iterations: 10,
                hashAlgorithm: HashAlgorithmName.SHA512,
                outputLength: 32); // Augment� � 32 pour une s�curit� accrue
            return Convert.ToHexString(hash);
        }

        public async Task RegisterUserAsync(string username, string password, string role)
        {
            // V�rifiez si l'utilisateur existe d�j�
            var existingUser = await _userRepository.GetUsersByPseudo(username);

            if (existingUser != null)
            {
                throw new Exception("User already exists");
            }

            // G�n�rer un sel s�curis� (ex: 16 octets al�atoires encod�s en base64)
            var salt = GenerateSalt();
            var passwordHash = HashPassword(password, salt);

            // Cr�er un nouvel utilisateur
            var newUser = new UsersData
            {
                Pseudo = username,
                Passwd = passwordHash,
                Salt = salt,
                Roles = role // Assurez-vous que le r�le est stock� sous forme de cha�ne appropri�e
            };

            // Ajouter l'utilisateur � la base de donn�es
            await _userRepository.AddAsync(newUser);
        }

        private string GenerateSalt()
        {
            // G�n�re un sel al�atoire de mani�re s�curis�e
            using (var rng = new RNGCryptoServiceProvider())
            {
                var saltBytes = new byte[16]; // Utilisez une taille appropri�e pour le sel
                rng.GetBytes(saltBytes);
                return Convert.ToBase64String(saltBytes);
            }
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            // R�cup�re l'utilisateur par pseudo
            var user = await _userRepository.GetUsersByPseudo(username) ??
                       throw new Exception("invalide utilisateur ou password");

            // V�rifie le mot de passe
            var passwordHash = HashPassword(password, user.Salt);
            if (user.Passwd != passwordHash)
            {
                throw new Exception("erreur login , user ou password incorrect");
            }

            // Convertit le r�le en liste (au cas o� il y aurait plusieurs r�les)
            var roles = new List<string> { user.Roles };

            // G�n�re le token JWT
            var token = GenerateJSONWebToken(username, roles);
            return token;
        }

    }

    //public class User
    //{
    //    public string Username { get; set; }
    //    public string Password { get; set; }
    //    public string Salt { get; set; }
    //    public string Role { get; set; }

    //    public User(string username, string password, string salt, string role)
    //    {
    //        Username = username;
    //        Password = password;
    //        Salt = salt;
    //        Role = role;
    //    }


    // Interface pour le d�p�t des utilisateurs
    //public interface IUserRepository
    //{
    //    Task<bool> UserExistsAsync(string username);
    //    Task AddUserAsync(User user);
    //    Task<User> GetUserByUsernameAsync(string username);
    //    Task<List<string>> GetUserRolesAsync(User user); // R�cup�re les r�les de l'utilisateur
    //}
}
