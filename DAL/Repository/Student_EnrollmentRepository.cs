﻿using DAL.Data;
using DAL.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Tools;
using System.Collections;

namespace DAL.Repository
{
    public class Student_EnrollmentRepository : RepositoryBase, IStudent_EnrollmentRepository
    {
        private readonly EFDbContextData _context;

        public Student_EnrollmentRepository(Connection connection, EFDbContextData context) : base(connection)
        {
            _context = context;
        }

        // Méthode pour récupérer les cours pour un étudiant spécifique en utilisant l'ID de l'étudiant
        public async Task<IEnumerable<CoursData>> CoursPourchaqueEtuAsync(int userId)
        {
            return await (from enrollment in _context.StudentEnrollements
                          join course in _context.Courses on enrollment.CoursId equals course.Id
                          where enrollment.UserId == userId
                          select new CoursData
                          {
                              Id = course.Id,
                              Nom = course.Nom,
                              // Ajoutez d'autres propriétés si nécessaire
                          }).ToListAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var enrollment = await _context.StudentEnrollements.FindAsync(id);
            if (enrollment == null)
            {
                throw new KeyNotFoundException($"No enrollment found with ID {id}");
            }

            _context.StudentEnrollements.Remove(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UsersData>> GetUsersWithCoursesAsync()
        {
            return await _context.Users
                .Include(u => u.StudentEnrollements) // Inclure les cours auxquels l'utilisateur est inscrit
                .ToListAsync();
        }
        public async Task<IEnumerable<CoursData>> EnrolledStudent(int id)
        {
            return await (from enrollment in _context.StudentEnrollements
                          join course in _context.Courses on enrollment.CoursId equals course.Id
                          where enrollment.UserId == id
                          select new CoursData
                          {
                              Id = course.Id,
                              Nom = course.Nom,
                              date_debut = course.date_debut,
                              date_fin = course.date_fin,
                              Disponible = course.Disponible
                              // Ajoutez d'autres propriétés si nécessaire
                          }).ToListAsync();
        }

        public Task EnrollementCoursesAsync(int userid, int courseid)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Student_EnrollementData>> GetAllGradesAsync()
        {
            return await _context.StudentEnrollements
                .Include(se => se.User)
                .Include(se => se.Cours)
                .ToListAsync();
        }



        // Méthode pour récupérer tous les utilisateurs inscrits à un cours spécifique en utilisant l'ID du cours
        public async Task<IEnumerable<UsersData>> GetAlluserBycourse(int id)
        {
            // Assurez-vous que l'ID du cours est valide
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

        public async Task<IEnumerable<Student_EnrollementData>> GetByCourseAsync(int courseId)
        {
            var enrollments = await _context.StudentEnrollements
                .Include(se => se.User)
                .Include(se => se.Cours)
                .Where(se => se.CoursId == courseId)
                .ToListAsync();

            if (!enrollments.Any())
            {
                throw new KeyNotFoundException($"No enrollments found for course ID {courseId}");
            }

            return enrollments;
        }



        public async Task<IEnumerable<Student_EnrollementData>> GetByUserIdAsync(int userId)
        {
            var enrollments = await _context.StudentEnrollements
                .Include(se => se.User)
                .Include(se => se.Cours)
                .Where(se => se.UserId == userId)
                .ToListAsync();

            if (!enrollments.Any())
            {
                throw new KeyNotFoundException($"No enrollments found for user ID {userId}");
            }

            return enrollments;
        }
        public async Task InsertGrade(int id, int grade)
        {
            var enrollment = await _context.StudentEnrollements.FindAsync(id);
            if (enrollment == null)
            {
                throw new KeyNotFoundException($"No enrollment found with ID {id}");
            }

            enrollment.Grade = grade;
            await _context.SaveChangesAsync();
        }



        // Méthode pour insérer un cours pour un étudiant en utilisant les IDs
        public async Task InsertStudentCoursesAsync(int userId, int courseId)
        {
            try
            {
                // Vérifier si l'inscription existe déjà
                bool existingEnrollment = await _context.StudentEnrollements
                    .AnyAsync(e => e.UserId == userId && e.CoursId == courseId);

                if (existingEnrollment)
                {
                    throw new InvalidOperationException("L'étudiant est déjà inscrit à ce cours.");
                }

                // Créer une nouvelle inscription
                var enrollment = new Student_EnrollementData
                {
                    UserId = userId,
                    CoursId = courseId
                };

                // Ajouter l'inscription à la base de données
                await _context.StudentEnrollements.AddAsync(enrollment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Gestion des erreurs liées à la base de données
                Console.WriteLine("Erreur de mise à jour de la base de données : " + ex.Message);
                throw; // Propager l'exception
            }
            catch (Exception ex)
            {
                // Gestion des autres erreurs
                Console.WriteLine("Une erreur est survenue : " + ex.Message);

                throw; // Propager l'exception
            }
        }

        // Supprimer cette méthode si elle est redondante
        public async Task InsertStudentCourseAsync(UsersData user, CoursData course)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "L'objet utilisateur ne peut pas être null.");

            if (course == null)
                throw new ArgumentNullException(nameof(course), "L'objet cours ne peut pas être null.");

            try
            {
                // Vérifier si l'inscription existe déjà
                bool existingEnrollment = await _context.StudentEnrollements
                    .AnyAsync(e => e.UserId == user.Id && e.CoursId == course.Id);

                if (existingEnrollment)
                {
                    throw new InvalidOperationException("L'étudiant est déjà inscrit à ce cours.");
                }

                // Créer une nouvelle inscription
                var enrollment = new Student_EnrollementData
                {
                    UserId = user.Id,
                    CoursId = course.Id
                };

                // Ajouter l'inscription à la base de données
                await _context.StudentEnrollements.AddAsync(enrollment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Gestion des erreurs liées à la base de données
                Console.WriteLine("Erreur de mise à jour de la base de données : " + ex.Message);
                throw; // Rejeter l'exception pour propager l'erreur
            }
            catch (Exception ex)
            {
                // Gestion des autres erreurs
                Console.WriteLine("Une erreur est survenue : " + ex.Message);
                throw; // Rejeter l'exception pour propager l'erreur
            }
        }
        // Assurez-vous d'utiliser cet espace de noms

        public async Task InsertStudentCourseAsync2(int userId, int courseId)
        {
            if (userId <= 0)
                throw new ArgumentException("L'identifiant utilisateur doit être supérieur à zéro.", nameof(userId));

            if (courseId <= 0)
                throw new ArgumentException("L'identifiant du cours doit être supérieur à zéro.", nameof(courseId));

            try
            {

                var userIdParam = new SqlParameter("@UserId", userId);
                var courseIdParam = new SqlParameter("@CourseId", courseId);


                await _context.Database.ExecuteSqlRawAsync(
                    "InsertStudentEnrollment @UserId, @CourseId",
                    userIdParam,
                    courseIdParam
                );
            }
            catch (SqlException ex)
            {

                Console.WriteLine("Erreur SQL : " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur est survenue : " + ex.Message);
                throw;
            }
        }

        public async Task UpdateGrade(Student_EnrollementData student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student), "L'objet étudiant ne peut pas être null.");
            }

            var existingEnrollment = await _context.StudentEnrollements.FindAsync(student.Id);
            if (existingEnrollment == null)
            {
                throw new KeyNotFoundException($"No enrollment found with ID {student.Id}");
            }

            existingEnrollment.Grade = student.Grade;
            _context.StudentEnrollements.Update(existingEnrollment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGrade(int id, int grade)
        {

            var enrollment = await _context.StudentEnrollements.FindAsync(id);

            if (enrollment == null)
            {
                throw new KeyNotFoundException("Enrollment not found.");
            }


            enrollment.Grade = grade;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateGradeAsync(int userId, int courseId, int grade)
        {

            var enrollment = await _context.StudentEnrollements
                .FirstOrDefaultAsync(se => se.UserId == userId && se.CoursId == courseId);

            if (enrollment == null)
            {
                return false;
            }
            enrollment.Grade = grade;


            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateGradesAsync(int userId, int courseId, int grade)
        {

            var enrollment = await _context.StudentEnrollements
                .FirstOrDefaultAsync(se => se.UserId == userId && se.CoursId == courseId);

            if (enrollment == null)
            {

                return false;
            }


            enrollment.Grade = grade;


            _context.StudentEnrollements.Update(enrollment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<UsersData>> GetStudentsWithCourses()
        {
            return await _context.Users
      .Include(u => u.StudentEnrollements) // Inclut les inscriptions des étudiants
          .ThenInclude(se => se.Cours)    // Puis inclut les cours associés
      .Where(u => u.Roles == "Etudiant") // Filtre pour obtenir uniquement les étudiants
      .ToListAsync();
        }


        public async Task<IEnumerable<UsersData>> GetInstructorsWithCourses()
        {
            return await _context.Users
       .Include(u => u.InstructorAssignments) // Inclut les assignations des instructeurs
           .ThenInclude(ia => ia.Cours)      // Puis inclut les cours associés
       .Where(u => u.Roles == "Professeur") // Filtre pour obtenir uniquement les instructeurs
       .ToListAsync();
        }
        public async Task<CoursData> GetCoursWithUsersAsync(int coursId)
        {
            return await _context.Courses
       .Include(c => c.StudentEnrollements)
           .ThenInclude(se => se.User)  // Inclure les utilisateurs inscrits au cours
       .FirstOrDefaultAsync(c => c.Id == coursId);
        }

        public async Task<UsersData> GetUserWithCoursesAsync(int userId)
        {
            return await _context.Users
        .Include(u => u.StudentEnrollements)
            .ThenInclude(se => se.Cours)  // Inclure les cours auxquels l'étudiant est inscrit
        .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<UsersData>> GetAllStudentsWithCoursesAsync()
        {
            return await _context.Users
             .Where(u => u.Roles == "Etudiant")
             .Include(u => u.StudentEnrollements)
                 .ThenInclude(se => se.Cours)
             .ToListAsync();
        }

        public async Task<List<UsersData>> GetAllProfessorsWithCoursesAsync()
        {
            return await _context.Users
            .Where(u => u.Roles == "Professeur")
            .Include(u => u.Courses)
            .ToListAsync();
        }

        public async Task<UsersData> GetProfessorWithCoursesAsync1(int professorId)
        {
            return await _context.Users
            .Where(u => u.Roles == "Professeur" && u.Id == professorId)
            .Include(u => u.Courses)
            .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<UsersData>> SearchUser(string search)
        {
            var users = await _context.Users
                .Where(u => u.Nom.Contains(search) || u.Prenom.Contains(search))
                .ToListAsync();

            return users;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
