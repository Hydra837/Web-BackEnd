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

namespace  BLL.Service//Authentication.Authentication
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
            Console.WriteLine(" user " + user.Password, user.Nom);
            UsersData a = await _userRepository.GetUsersByPseudo(user.Pseudo);
            if (a is not null)
            {
                throw new Exception("User already exists");
            }

            var salt = GenerateSalt();
            Console.WriteLine("salt " + salt);
            var passwordHash = HashPassword(user.Password, salt);

            var newUser = new UsersData
            {
                Pseudo = user.Password,
                Salt = salt,
                Roles = user.Role,
                Mail = user.Mail,
                Nom = user.Nom,
                Prenom = user.Prenom
            };

            await _userRepository.AddAsync(newUser);
        }
        //public void RegisterUser(string username, string password)
        //{
        //    if (users.Any(user => user.Username.ToLower() == username.ToLower()))
        //    {
        //        throw new Exception("User already exist");
        //    }
        //    var salt = DateTime.Now.ToString("dddd"); // get the day of week. Ex: Sunday
        //    var passwordHash = HashPassword(password, salt);
        //    var newUser = new User(username, passwordHash, salt);
        //    users.Add(newUser);s

        //   }

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
          
            UsersData user = await _userRepository.GetUsersByPseudo(username)
                      ?? throw new Exception("Login failed; Invalid username or password");

         
            UsersModel a = user.ToUserBLL();

       
            var passwordHash = HashPassword(password, a.Salt);


            if (a.Password != passwordHash)
            {
                throw new Exception("Login failed; Invalid username or password");
            }

           
            var roles = new List<string> { a.Role };

      
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
    }

}
