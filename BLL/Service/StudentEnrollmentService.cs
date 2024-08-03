using BLL.Interface;
using BLL.Mapper;
using BLL.Models;
using DAL.Data;
using DAL.Interface;
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

     
    

        public async Task<Student_EnrollmentModel> GetByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), "User ID must be greater than zero.");

            try
            {
                var enrollmentData = await _studentEnrollmentRepository.GetByUserIdAsync(userId);

                if (enrollmentData == null)
                    return null;

                return enrollmentData.ToEnrollementBLL();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error retrieving enrollment by user ID: {ex.Message}");

                // Optionally, rethrow the exception if it should be handled by a higher-level handler
                throw;
            }
        }

        public async Task<Student_EnrollmentModel> GetByCourseAsync(int courseId)
        {
            if (courseId <= 0)
                throw new ArgumentOutOfRangeException(nameof(courseId), "Course ID must be greater than zero.");

            try
            {
                var enrollmentData = await _studentEnrollmentRepository.GetByCourseAsync(courseId);

                if (enrollmentData == null)
                    return null;

                return enrollmentData.ToEnrollementBLL();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error retrieving enrollment by course ID: {ex.Message}");

                // Optionally, rethrow the exception if it should be handled by a higher-level handler
                throw;
            }
        }

        // Supprimer les méthodes non implémentées qui font doublon









    }
}
