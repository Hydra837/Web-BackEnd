using DAL.Data;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class CoursRepository : RepositoryBase, ICoursRepository
    {
        private readonly Connection _connection;
        private readonly EFDbContextData _context;
        protected readonly DbSet<CoursData> _dbSet;
        protected readonly DbSet<UsersData> _user;

        public CoursRepository(Connection connection, EFDbContextData context) : base(connection)
        {
            _connection = connection;
            _context = context;
            _dbSet = _context.Set<CoursData>();
            _user = _context.Set<UsersData>();

        }

        public async Task AddAsync(CoursData entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CoursData>> GetAllAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<IEnumerable<CoursData>> GetAllAvailable()
        {
            return await _context.Courses
                .Where(c => c.Disponible)
                .ToListAsync();
        }

        public async Task<CoursData> GetByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task UpdateAsync(CoursData entity)
        {
            var existingEntity = await _context.Courses.FindAsync(entity.Id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }

        public IEnumerable<CoursData> GetAllAvailble1()
        {
            return _context.Courses.Where(c => c.Disponible).ToList();
        }

        public async Task<IEnumerable<CoursData>> Getall()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task InsertUserCourseAsync(CoursData cours, UsersData user)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == user.Id);
            var courseExists = await _context.Courses.AnyAsync(c => c.Id == cours.Id);

            if (!userExists)
            {
                throw new ArgumentException("L'utilisateur avec cet ID n'existe pas.");
            }

            if (!courseExists)
            {
                throw new ArgumentException("Le cours avec cet ID n'existe pas.");
            }

            var enrollment = new Student_EnrollementData
            {
                UserId = user.Id,
                CoursId = cours.Id
            };

            await _context.StudentEnrollements.AddAsync(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CoursData>> GetallByUser(int userId)
        {
            // Assurez-vous que le userId est valide
            if (userId <= 0)
            {
                throw new ArgumentException("L'ID utilisateur doit être un nombre positif.", nameof(userId));
            }

            // Récupérez les cours auxquels l'utilisateur est inscrit
            var coursList = await _context.StudentEnrollements
                .Where(enrollment => enrollment.UserId == userId)
                .Select(enrollment => enrollment.Cours)
                .ToListAsync();

            return coursList;
        }

        public async Task<IEnumerable<UsersData>> GetAllUserBycourse1(int id)
        {
            // Assurez-vous que l'ID du cours est valide
            if (id <= 0)
            {
                throw new ArgumentException("L'ID du cours doit être un nombre positif.", nameof(id));
            }

            // Récupérez les utilisateurs inscrits à ce cours
            var usersList = await _context.StudentEnrollements
                .Where(enrollment => enrollment.CoursId == id)
                .Select(enrollment => enrollment.User)
                .ToListAsync();

            return usersList;
        }


        public Task<IEnumerable<UsersData>> GetAllUuserBycourse(int id)
        {
            throw new NotImplementedException();
        }
    }
}


//    public void Create(CoursData cours)
//    {
//        // ADO
//        // Valider les données du cours 
//        ValidateCours(cours);

//        using ( SqlConnection connection = _connection.CreateConnection())
//        {
//            connection.Open();

//            using (SqlCommand command = connection.CreateCommand())
//            {
//                command.CommandType = CommandType.StoredProcedure;
//                command.CommandText = "AddCours";

//                // Ajouter les paramètres
//                command.Parameters.Add(new SqlParameter("@Nom", SqlDbType.NVarChar, 100) { Value = cours.Nom });
//                command.Parameters.Add(new SqlParameter("@DateDebut", SqlDbType.DateTime) { Value = cours.DateDebut });
//                command.Parameters.Add(new SqlParameter("@DateFin", SqlDbType.DateTime) { Value = cours.DateFin });

//                try
//                {
//                    command.ExecuteNonQuery();
//                }
//                catch (SqlException ex)
//                {
//                    // Gestion des erreurs SQL
//                    Console.WriteLine("Erreur SQL : " + ex.Message);
//                    // Loggez l'erreur (par exemple, dans un fichier ou un système de journalisation)
//                    throw;
//                }
//                catch (Exception ex)
//                {
//                    // Gestion des autres erreurs
//                    Console.WriteLine("Une erreur est survenue : " + ex.Message);
//                    // Loggez l'erreur (par exemple, dans un fichier ou un système de journalisation)
//                    throw;
//                }
//            }
//        }
//    }

//    private void ValidateCours(CoursData cours)
//    {
//        if (string.IsNullOrWhiteSpace(cours.Nom))
//        {
//            throw new ValidationException("Le nom du cours est requis.");
//        }

//        if (cours.DateDebut >= cours.DateFin)
//        {
//            throw new ValidationException("La date de début doit être antérieure à la date de fin.");
//        }

//        // Ajoutez d'autres validations si nécessaire
//    }


//public void Delete(int id)
//    {

//        using (SqlConnection connection = _connection.CreateConnection())
//        {
//            try
//            {
//                connection.Open();

//                using (SqlCommand command = connection.CreateCommand())
//                {
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.CommandText = "delete_cours";

//                    // Ajouter le paramètre @Id
//                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

//                    // Exécuter la procédure stockée
//                    int rowsAffected = command.ExecuteNonQuery();

//                    if (rowsAffected > 0)
//                    {
//                        Console.WriteLine($"Le cours avec Id = {id} a été supprimé avec succès.");
//                    }
//                    else
//                    {
//                        Console.WriteLine($"Aucun cours trouvé avec Id = {id}.");
//                    }
//                }
//            }
//            catch (SqlException ex)
//            {
//                // Gestion des erreurs SQL
//                Console.WriteLine("Erreur SQL : " + ex.Message);
//                // Loggez l'erreur (par exemple, dans un fichier ou un système de journalisation)
//                throw;
//            }
//            catch (Exception ex)
//            {
//                // Gestion des autres erreurs
//                Console.WriteLine("Une erreur est survenue : " + ex.Message);
//                // Loggez l'erreur (par exemple, dans un fichier ou un système de journalisation)
//                throw;
//            }
//        }
//    }

//    public IEnumerable<CoursData> GetAllAvailble()
//    {
//        using (SqlConnection connection = _connection.CreateConnection())
//        {
//            try
//            {
//                connection.Open();

//                // Exécuter la procédure stockée avec Dapper
//                var cours = connection.Query<CoursData>("GetAvailableCours", commandType: CommandType.StoredProcedure);

//                return cours;
//            }
//            catch (SqlException ex)
//            {
//                // Gestion des erreurs SQL
//                Console.WriteLine("Erreur SQL : " + ex.Message);
//                // Loggez l'erreur (par exemple, dans un fichier ou un système de journalisation)
//                throw;
//            }
//            catch (Exception ex)
//            {
//                // Gestion des autres erreurs
//                Console.WriteLine("Une erreur est survenue : " + ex.Message);
//                // Loggez l'erreur (par exemple, dans un fichier ou un système de journalisation)
//                throw;
//            }
//        }

//    }

//    public void InsertUserCourse(int id, int id_cours)
//    {
//        using (var connection = _connection.CreateConnection())
//        {
//            try
//            {
//                connection.Open();

//                // Préparation des paramètres
//                var parameters = new DynamicParameters();
//                parameters.Add("@CoursId", id_cours, DbType.Int32);
//                parameters.Add("@UserId", id, DbType.Int32);

//                // Exécution de la procédure stockée avec Dapper
//                int enrollmentId = connection.QuerySingle<int>("AddStudentEnrollment", parameters, commandType: CommandType.StoredProcedure);

//             //   return enrollmentId;
//            }
//            catch (SqlException ex)
//            {
//                // Gestion des erreurs SQL
//                Console.WriteLine("Erreur SQL : " + ex.Message);
//                // Loggez l'erreur (par exemple, dans un fichier ou un système de journalisation)
//                throw;
//            }
//            catch (Exception ex)
//            {
//                // Gestion des autres erreurs
//                Console.WriteLine("Une erreur est survenue : " + ex.Message);
//                // Loggez l'erreur (par exemple, dans un fichier ou un système de journalisation)
//                throw;
//            }
//        }
//    }

//    public void Update(CoursData cours)
//    {
//        using (var connection = _connection.CreateConnection())
//        {
//            try
//            {
//                connection.Open();

//                // Préparation des paramètres
//                var parameters = new DynamicParameters();
//                parameters.Add("@Id", cours.Id, DbType.Int32);
//                parameters.Add("@Nom", cours.Nom, DbType.String);
//                parameters.Add("@DateDebut", cours.DateDebut, DbType.DateTime);
//                parameters.Add("@DateFin", cours.DateFin, DbType.DateTime);

//                // Exécution de la procédure stockée avec Dapper
//                int rowsAffected = connection.Execute("UpdateCours", parameters, commandType: CommandType.StoredProcedure);

//             //   return rowsAffected; SI JE VEUX CHANGER INTERFACE POUR QU IL RENVOIE LA VALEUR RENT2RER
//            }
//            catch (SqlException ex)
//            {
//                // Gestion des erreurs SQL
//                Console.WriteLine("Erreur SQL : " + ex.Message);
//                // Loggez l'erreur (par exemple, dans un fichier ou un système de journalisation)
//                throw;
//            }
//            catch (Exception ex)
//            {
//                // Gestion des autres erreurs
//                Console.WriteLine("Une erreur est survenue : " + ex.Message);
//                // Loggez l'erreur (par exemple, dans un fichier ou un système de journalisation)
//                throw;
//            }
//        }
//    }

//    public void Update(int id)
//    {
//        throw new NotImplementedException();
//    }
