using DAL.Data;
using DAL.Interface;
using DAL.Mapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tools;

namespace DAL.Repository
{
    public class UsersRepository : RepositoryBase, IusersRepository
    {
        private readonly Connection _connection;
        private readonly EFDbContextData _context;
        private readonly DbSet<UsersData> _dbSet;
        private readonly DbSet<UserCourseDetailsData> _detailsDbSet;
        private readonly ILogger<UsersRepository> _logger;

        public UsersRepository(Connection connection, EFDbContextData context, ILogger<UsersRepository> logger) : base(connection)
        {
            _connection = connection;
            _context = context;
            _dbSet = _context.Users;
            _detailsDbSet = _context.UserCourseDetails;
            _logger = logger;
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UsersData>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all users");
                throw;
            }
        }

        public async Task<UsersData> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving user with id {id}");
                throw;
            }
        }

        public async Task AddAsync(UsersData entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new user");
                throw;
            }
        }

        public async Task UpdateAsync(UsersData entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user");
                throw;
            }
        }

        // Optional: Implement a method to delete a user if required
        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting user with id {id}");
                throw;
            }
        }
        public void UserRole(int userId, string role)
        {
            using (var connection = _connection.CreateConnection())
            {
                connection.Open();

               
                string roleQuery = "SELECT Id FROM Roles WHERE RoleName = @RoleName";
                int? roleId = connection.QueryFirstOrDefault<int?>(roleQuery, new { RoleName = role });

                if (roleId == null)
                {
                   
                    string insertRoleQuery = "INSERT INTO Roles (RoleName) VALUES (@RoleName); SELECT CAST(SCOPE_IDENTITY() as int)";
                    roleId = connection.ExecuteScalar<int>(insertRoleQuery, new { RoleName = role });
                }

              
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
            var userCourseDetails = await _detailsDbSet.FromSqlRaw(sql).ToListAsync();
       
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
