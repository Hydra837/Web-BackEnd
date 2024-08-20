using BLL.Interface;
using BLL.Mapper;
using BLL.Models;
using DAL.Data;
using DAL.Interface;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class StudentEnrollmentService : IStudentEnrollmentService
    {
        private readonly IStudent_EnrollmentRepository _studentEnrollmentRepository;

        public StudentEnrollmentService(IStudent_EnrollmentRepository studentEnrollmentRepository)
        {
            _studentEnrollmentRepository = studentEnrollmentRepository;
        }

       
        public async Task<IEnumerable<CoursModel>> CoursPourchaqueEtuAsync(int studentId)
        {
            if (studentId <= 0)
                throw new ArgumentOutOfRangeException(nameof(studentId), "Student ID must be greater than zero.");

            try
            {
                var courses = await _studentEnrollmentRepository.CoursPourchaqueEtuAsync(studentId);

                if (courses == null || !courses.Any())
                    return Enumerable.Empty<CoursModel>();

                return courses.Select(course => course.ToCoursBLL());
            }
            catch (Exception ex)
            {
           
                Console.WriteLine($"Error retrieving courses for student: {ex.Message}");

        
                throw;
            }
        }
        //public async Task<IEnumerable<UsersModel>> GetUsersWithCoursesAsync()
        //{
        //    //return await _userRepository.GetUsersWithCoursesAsync();
            
        //}
    
    // Méthode asynchrone pour insérer un cours pour un étudiant en utilisant des IDs
    public async Task InsertStudentCourseAsync2(int studentId, int courseId)
        {
            if (studentId <= 0)
                throw new ArgumentOutOfRangeException(nameof(studentId), "Student ID must be greater than zero.");

            if (courseId <= 0)
                throw new ArgumentOutOfRangeException(nameof(courseId), "Course ID must be greater than zero.");

            try
            {
                await _studentEnrollmentRepository.InsertStudentCourseAsync2(studentId, courseId);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error inserting student course: {ex.Message}");

                // Optionally, rethrow the exception if it should be handled by a higher-level handler
                throw;
            }
        }

        // Nouvelle méthode asynchrone pour insérer un cours pour un étudiant en utilisant des objets complets
        public async Task InsertStudentCourseAsync(UsersModel user, CoursModel course)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "L'objet utilisateur ne peut pas être null.");

            if (course == null)
                throw new ArgumentNullException(nameof(course), "L'objet cours ne peut pas être null.");

            try
            {
                var userData = user.ToUserDAL();
                var courseData = course.ToCoursDAL();
                await _studentEnrollmentRepository.InsertStudentCourseAsync(userData, courseData);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error inserting student course: {ex.Message}");

                // Optionally, rethrow the exception if it should be handled by a higher-level handler
                throw;
            }
        }

        // Méthode asynchrone pour récupérer tous les utilisateurs inscrits à un cours
        public async Task<IEnumerable<UsersModel>> GetAlluserBycourse(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Course ID must be greater than zero.");

            try
            {
                var usersData = await _studentEnrollmentRepository.GetAlluserBycourse(id);

                if (usersData == null || !usersData.Any())
                    return Enumerable.Empty<UsersModel>();

                return usersData.Select(user => user.ToUserBLL());
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error retrieving users for course: {ex.Message}");

                // Optionally, rethrow the exception if it should be handled by a higher-level handler
                throw;
            }
        }

        // Méthode asynchrone pour récupérer les cours auxquels un étudiant est inscrit
        public async Task<IEnumerable<CoursModel>> EnrolledStudentAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Student ID must be greater than zero.");

            try
            {
                var coursesData = await _studentEnrollmentRepository.EnrolledStudent(id);

                if (coursesData == null || !coursesData.Any())
                    return Enumerable.Empty<CoursModel>();

                return coursesData.Select(course => course.ToCoursBLL());
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error retrieving enrolled courses for student: {ex.Message}");

                // Optionally, rethrow the exception if it should be handled by a higher-level handler
                throw;
            }
        }

        // Méthode asynchrone pour récupérer toutes les notes des étudiants
        public async Task<IEnumerable<Student_EnrollmentModel>> GetAllGradesAsync()
        {
            try
            {
                var enrollmentsData = await _studentEnrollmentRepository.GetAllGradesAsync();

                if (enrollmentsData == null || !enrollmentsData.Any())
                    return Enumerable.Empty<Student_EnrollmentModel>();

                return enrollmentsData.Select(enrollment => enrollment.ToEnrollementBLL());
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error retrieving all grades: {ex.Message}");

                // Optionally, rethrow the exception if it should be handled by a higher-level handler
                throw;
            }
        }

        // Méthode asynchrone pour insérer une note pour un étudiant
        public async Task InsertGrade(int id, int grade)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Enrollment ID must be greater than zero.");

            try
            {
                await _studentEnrollmentRepository.InsertGrade(id, grade);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error inserting grade: {ex.Message}");

                // Optionally, rethrow the exception if it should be handled by a higher-level handler
                throw;
            }
        }

        // Méthode asynchrone pour mettre à jour une note pour un étudiant
        public async Task UpdateGrade(Student_EnrollmentModel student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student), "L'objet étudiant ne peut pas être null.");

            try
            {
                var enrollmentData = student.ToEnrollementDAL();
                await _studentEnrollmentRepository.UpdateGrade(enrollmentData);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error updating grade: {ex.Message}");

                // Optionally, rethrow the exception if it should be handled by a higher-level handler
                throw;
            }
        }

        // Méthode asynchrone pour supprimer une inscription
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Enrollment ID must be greater than zero.");

            try
            {
                await _studentEnrollmentRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error deleting enrollment: {ex.Message}");

                // Optionally, rethrow the exception if it should be handled by a higher-level handler
                throw;
            }
        }

        public void InsertStudentCourse(int id, int id_Course)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CoursModel> CoursPourchaqueEtu()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UsersModel>> GetUsersWithCoursesAsync()
        {
            IEnumerable<UsersData> a = await _studentEnrollmentRepository.GetUsersWithCoursesAsync();
            return a.Select(x => x.ToUserBLL());
        }

        public async Task<IEnumerable<Student_EnrollmentModel>> GetByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), "User ID must be greater than zero.");

            try
            {
                var enrollmentData = await _studentEnrollmentRepository.GetByUserIdAsync(userId);

                if (enrollmentData == null || !enrollmentData.Any())
                    return Enumerable.Empty<Student_EnrollmentModel>();

                return enrollmentData.Select(x => x.ToEnrollementBLL());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving enrollment by user ID: {ex.Message}");
                throw;
            }
        }



        public async Task<IEnumerable<Student_EnrollmentModel>> GetByCourseAsync(int courseId)
        {
            if (courseId <= 0)
                throw new ArgumentOutOfRangeException(nameof(courseId), "Course ID must be greater than zero.");

            try
            {
                var enrollmentData = await _studentEnrollmentRepository.GetByCourseAsync(courseId);

                if (enrollmentData == null || !enrollmentData.Any())
                    return Enumerable.Empty<Student_EnrollmentModel>();

                return enrollmentData.Select(x => x.ToEnrollementBLL());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving enrollment by course ID: {ex.Message}");
                throw;
            }
        }
  

        public async Task<bool> IsUserEnrolledInCourseAsync(int userId, int courseId)
        {
           
            IEnumerable<Student_EnrollementData> enrollments = await _studentEnrollmentRepository.GetByUserIdAsync(userId);

            
            foreach (Student_EnrollementData enrollment in enrollments)
            {
                if (enrollment.CoursId == courseId)
                {
                    return false; 
                }
            }

            return true; 
        }


        public async Task<bool> UpdateGrade(int id, int grade)
        {
            try
            {
                await _studentEnrollmentRepository.UpdateGrade(id, grade);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateGradesAsync(int userId, int courseId, int grade)
        {
            return await _studentEnrollmentRepository.UpdateGradesAsync(userId, courseId, grade);
        }

        public async Task<ICollection<UsersModel>> GetStudentAllCourseAsync()
        {
            var coursesData = await _studentEnrollmentRepository.GetStudentsWithCourses();
            //  ICollection<CoursModel> c = coursesData.Select(x => x.ToUserBLL()).ToList();
            return coursesData.Select(x => x.ToUserBLL()).ToList();
        }

        public async Task<ICollection<UsersModel>> GetTeacherAllCourseAsync()
        {
            var coursesModel = await _studentEnrollmentRepository.GetInstructorsWithCourses();
            return coursesModel.Select(x => x.ToUserBLL()).ToList();
        }
        public async Task<CoursModel> GetCoursWithUsersAsync1(int coursId)
        {
            var cours = await _studentEnrollmentRepository.GetCoursWithUsersAsync(coursId);

            if (cours == null)
            {
                return null;
            }

            return new CoursModel
            {
                Id = cours.Id,
                Nom = cours.Nom,
                Disponible = cours.Disponible,
                DateDebut = cours.date_debut,
                DateFin = cours.date_fin,
                Description = cours.Description,
                usersModels = cours.StudentEnrollements.Select(se => new UsersModel
                {
                    Id = se.User.Id,
                    Nom = se.User.Nom,
                    Prenom = se.User.Prenom
                }).ToList()
            };
        }

        public async Task<UsersModel> GetUserWithCoursesAsync1(int userId)
        {
            var user = await _studentEnrollmentRepository.GetUserWithCoursesAsync(userId);

            if (user == null)
            {
                return null;
            }

            return new UsersModel
            {
                Id = user.Id,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Role = user.Roles,
                Cours = user.StudentEnrollements.Select(se => new CoursModel
                {
                    Id = se.Cours.Id,
                    Nom = se.Cours.Nom
                }).ToList()
            };
        }

        public async Task<List<UsersModel>> GetAllStudentsWithCoursesAsync()
        {
            var students = await _studentEnrollmentRepository.GetAllStudentsWithCoursesAsync();

            return students.Select(u => new UsersModel
            {
                Id = u.Id,
                Nom = u.Nom,
                Prenom = u.Prenom,
                Role = u.Roles,
                Cours = u.StudentEnrollements.Select(se => new CoursModel
                {
                    Id = se.Cours.Id,
                    Nom = se.Cours.Nom
                }).ToList()
            }).ToList();
        }

        public async Task<List<UsersModel>> GetAllProfessorsWithCoursesAsync()
        {
            var professors = await _studentEnrollmentRepository.GetAllProfessorsWithCoursesAsync();

            return professors.Select(p => new UsersModel
            {
                Id = p.Id,
                Nom = p.Nom,
                Prenom = p.Prenom,
                Role = p.Roles,
                Cours = p.Courses.Select(c => new CoursModel
                {
                    Id = c.Id,
                    Nom = c.Nom
                }).ToList()
            }).ToList();
        }

        public async Task<UsersModel> GetProfessorWithCoursesAsync(int professorId)
        {
            var professor = await _studentEnrollmentRepository.GetProfessorWithCoursesAsync1(professorId);

            if (professor == null) return null;

            return new UsersModel
            {
                Id = professor.Id,
                Nom = professor.Nom,
                Prenom = professor.Prenom,
                Role = professor.Roles,
                Cours = professor.Courses.Select(c => new CoursModel
                {
                    Id = c.Id,
                    Nom = c.Nom
                }).ToList()
            };

        }

        public Task<List<UsersModel>> GetUsersWithCoursesAssignmentsAndGradesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
