using DAL.Data;
using DAL.Interface;
using DAL.Mapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace DAL.Repository
{
    public class UsersRepository : RepositoryBase, IusersRepository
    {
        private readonly Connection _connection;
        private readonly EFDbContextData _context;
        private readonly DbSet<UsersData> _dbSet;
        private readonly DbSet<UserCourseDetailsData> __db;
        public UsersRepository(Connection connection, EFDbContextData context) : base(connection)
        {
           _connection = connection;
            _context = context;
            _dbSet = _context.Users;
            __db = _context.UserCourseDetails;
           


        }

      

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UsersData>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<UsersData> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(UsersData entity)
        {
             await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync(); ;
        }

        public async Task UpdateAsync(UsersData entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new Exception($"l'utilisateur avec  l'ID {id} n'a pas été trouvé.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        public void UserRole(int userId, string role)
        {
            using (var connection = _connection.CreateConnection())
            {
                connection.Open();

                // Vérifier si le rôle existe
                string roleQuery = "SELECT Id FROM Roles WHERE RoleName = @RoleName";
                int? roleId = connection.QueryFirstOrDefault<int?>(roleQuery, new { RoleName = role });

                if (roleId == null)
                {
                    // Insérer le rôle s'il n'existe pas
                    string insertRoleQuery = "INSERT INTO Roles (RoleName) VALUES (@RoleName); SELECT CAST(SCOPE_IDENTITY() as int)";
                    roleId = connection.ExecuteScalar<int>(insertRoleQuery, new { RoleName = role });
                }

                // Assigner le rôle à l'utilisateur
                string userRoleQuery = "INSERT INTO UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)";
                connection.Execute(userRoleQuery, new { UserId = userId, RoleId = roleId });
            }
        }

       



        private string HashPassword(string password)
        {
            // Utilisez une méthode de hachage sécurisée pour le mot de passe
            // Par exemple, vous pouvez utiliser BCrypt, PBKDF2, ou une autre fonction de hachage sécurisée
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public void Register(string username, string password)
        {
            using (var connection = _connection.CreateConnection())
            {
                connection.Open();

                // Hacher le mot de passe avec BCrypt
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                string query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";

                var parameters = new
                {
                    Username = username,
                    Password = hashedPassword
                };

                connection.Execute(query, parameters);
            }
        }

        public UsersData Login(string username, string password)
        {
            using (var connection = _connection.CreateConnection())
            {
                connection.Open();

                string query = "SELECT * FROM Users WHERE Username = @Username";
                var user = connection.QueryFirstOrDefault<UsersData>(query, new { Username = username });

                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Passwd))
                {
                    return user;
                }

                return null; // Login échoué
            }
        }

        public void UserRole(string role)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserCourseDetailsData>> GetUsersCoursesAsync()
        {
            var sql = " GetUserCourseDetails";
            var userCourseDetails = await __db.FromSqlRaw(sql).ToListAsync();
       
            return userCourseDetails;
        }

        public Task LoginAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public Task SetUserRoleAsync(string username, string role)
        {
            throw new NotImplementedException();
        }

        public Task RegisterAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        void IusersRepository.Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CoursData>> GetallByUser(int userId)
        {
            var coursList = await _context.Courses
                .Where(uc => uc.Id == userId)
                .Select(uc => new CoursData
                {
                    Id = uc.Id,
                    Nom = uc.Nom,
                    date_debut = uc.date_debut,
                    date_fin = uc.date_fin,
                })
                .ToListAsync();

            return coursList;
        }

        public async Task<UsersData> GetUsersByPseudo(string pseudo)
        {
           
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Pseudo == pseudo);

            return user;
        }

    }
}
