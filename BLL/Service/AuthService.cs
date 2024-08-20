using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using DAL.Interface;
using BLL.Models;
using DAL.Data;
using BLL.Mapper;
using BLL.Interface;
using System.Linq;

namespace BLL.Service
{
    public class AuthService : IAuthenticationService
    {
        private readonly IConfiguration _config;
        private readonly IusersRepository _userRepository;

        public AuthService(IConfiguration config, IusersRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        private string GenerateJSONWebToken(string username, List<string> roles)
        {
            var secretKey = _config["Jwt:Key"];
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
                iterations: 10000,
                hashAlgorithm: HashAlgorithmName.SHA512,
                outputLength: 32);
            return Convert.ToHexString(hash);
        }

        public async Task RegisterUserAsync(UsersModel user)
        {
            var existingUser = await _userRepository.GetUsersByPseudo(user.Pseudo);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User already exists");
            }

            var salt = GenerateSalt();
            var passwordHash = HashPassword(user.Password, salt);

            var newUser = new UsersData
            {
                Pseudo = user.Pseudo, // Corrected from user.Password
                Salt = salt,
                Passwd = passwordHash,
                Roles = user.Role,
                Mail = user.Mail,
                Nom = user.Nom,
                Prenom = user.Prenom
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
        public async Task ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await  _userRepository.GetByIdAsync(userId)
                ?? throw new InvalidOperationException("User not found");

            var currentHashedPassword = HashPassword(currentPassword, user.Salt);

            if (user.Passwd != currentHashedPassword)
            {
                throw new InvalidOperationException("Current password is incorrect");
            }

            var newHashedPassword = HashPassword(newPassword, user.Salt);
            user.Passwd = newHashedPassword;

            _userRepository.UpdateAsync(user);
        }
        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetUsersByPseudo(username)
                      ?? throw new InvalidOperationException("Invalid username or password");

            var passwordHash = HashPassword(password, user.Salt);

            if (user.Passwd != passwordHash)
            {
                throw new InvalidOperationException("Invalid username or password");
            }

            var roles = new List<string> { user.Roles }; // Adjust as necessary
            return GenerateJSONWebToken(username, roles);
        }

        public string RefreshToken(string token)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserRoleAsync(string username)
        {
            throw new NotImplementedException();
        }

        public async Task ForgotPasswordAsync(string pseudo, string newPassword)
        {
            var user = await _userRepository.GetUsersByPseudo(pseudo)
               ?? throw new InvalidOperationException("User not found");

            var salt = GenerateSalt();
            user.Passwd = HashPassword(newPassword, salt);
            user.Salt = salt;

            _userRepository.UpdateAsync(user);
           // await _userRepository.SaveChangesAsync();

        }
    }
}
