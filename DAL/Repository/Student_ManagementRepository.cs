using DAL.Data;
using DAL.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools;

namespace DAL.Repository
{
    public class Student_ManagementRepository : RepositoryBase, IStudent_Management
    {
        private readonly EFDbContextData _context;

        public Student_ManagementRepository(Connection connection, EFDbContextData context) : base(connection)
        {
            _context = context;
        }

        public void Create(CoursData cours)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(CoursData cours)
        {
            try
            {
                await _context.Courses.AddAsync(cours);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Erreur de mise à jour de la base de données : " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur est survenue : " + ex.Message);
                throw;
            }
        }

        public Task CreateAsync(UsersData usersData)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int studentId, int courseId)
        {
            try
            {
                var enrollment = await _context.StudentEnrollements
                    .FirstOrDefaultAsync(e => e.UserId == studentId && e.CoursId == courseId);

                if (enrollment != null)
                {
                    _context.StudentEnrollements.Remove(enrollment);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Erreur de mise à jour de la base de données : " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur est survenue : " + ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var student = await _context.Users.FindAsync(id);
                if (student != null)
                {
                    _context.Users.Remove(student);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Erreur de mise à jour de la base de données : " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur est survenue : " + ex.Message);
                throw;
            }
        }

        public IEnumerable<CoursData> GetAllCourseByUser(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CoursData>> GetAllCourseByUserAsync(int userId)
        {
            return await (from enrollment in _context.StudentEnrollements
                          join course in _context.Courses on enrollment.CoursId equals course.Id
                          where enrollment.UserId == userId
                          select course).ToListAsync();
        }

 

        // Méthode pour insérer une nouvelle inscription dans la table Student_Management
        public async Task InsertUserCoursAsync(int id, int id_cours)
        {
            // Créez une nouvelle instance de Student_ManagementData avec les valeurs fournies
            var studentManagement = new Student_ManagementData
            {
                ProfesseurId = id,
                CoursId = id_cours,
               // EnrollmentDate = DateTime.Now // Assurez-vous d'ajouter d'autres champs si nécessaire
            };

            // Ajoutez l'entrée à la table Student_Management
            await _context.InstructorAssignments.AddAsync(studentManagement);

            // Sauvegardez les changements dans la base de données
            await _context.SaveChangesAsync();
        }

        public async Task InsertUserCourseAsync(int userId, int courseId)
        {
            try
            {
                var existingEnrollment = await _context.StudentEnrollements
                    .AnyAsync(e => e.UserId == userId && e.CoursId == courseId);

                if (existingEnrollment)
                {
                    throw new InvalidOperationException("L'étudiant est déjà inscrit à ce cours.");
                }

                var enrollment = new Student_EnrollementData
                {
                    UserId = userId,
                    CoursId = courseId
                };

                await _context.StudentEnrollements.AddAsync(enrollment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Erreur de mise à jour de la base de données : " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur est survenue : " + ex.Message);
                throw;
            }
        }

        public void Update(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(int id, UsersData updatedUserData)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(id);
                if (existingUser != null)
                {
                    _context.Entry(existingUser).CurrentValues.SetValues(updatedUserData);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Erreur de mise à jour de la base de données : " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur est survenue : " + ex.Message);
                throw;
            }
        }
    }
}
