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

        // Méthode synchrone : La méthode de récupération des cours pour chaque étudiant n'est pas encore implémentée
        public IEnumerable<CoursModel> CoursPourchaqueEtu()
        {
            throw new NotImplementedException();
        }

        // Méthode asynchrone pour récupérer les cours d'un étudiant spécifique
        //public async Task<IEnumerable<CoursModel>> CoursPourchaqueEtuAsync(int studentId)
        //{
        //    if (studentId <= 0)
        //        throw new ArgumentOutOfRangeException(nameof(studentId), "Student ID must be greater than zero.");

        //    try
        //    {
        //        var courses = await _studentEnrollmentRepository.CoursPourchaqueEtu(studentId);

        //        if (courses == null || !courses.Any())
        //            return Enumerable.Empty<CoursModel>();

        //        return courses.Select(x => x.ToCoursModel()); // Utilisation d'un mapper pour convertir les données
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        Console.WriteLine($"Error retrieving courses for student: {ex.Message}");

        //        // Optionally, rethrow the exception if it should be handled by a higher-level handler
        //        throw;
        //    }
        //}

        // Méthode synchrone pour insérer un cours pour un étudiant
        public void InsertStudentCourse(int studentId, int courseId)
        {
            throw new NotImplementedException();
        }

        // Méthode asynchrone pour insérer un cours pour un étudiant
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
                UsersData a = user.ToUserDAL();
                CoursData b = course.ToCoursDAL();
                await _studentEnrollmentRepository.InsertStudentCourseAsync(a, b);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error inserting student course: {ex.Message}");

                // Optionally, rethrow the exception if it should be handled by a higher-level handler
                throw;
            }
        }

        public Task InsertStudentCourseAsync(UsersModel user, CoursData course)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UsersModel>> GetallUsersByCourse1(int id)
        {
            // Assurez-vous que l'ID du cours est valide
            if (id <= 0)
            {
                throw new ArgumentException("L'ID du cours doit être un nombre positif.", nameof(id));
            }

            // Récupérez les utilisateurs inscrits à ce cours depuis le repository
            var usersDataList = await _studentEnrollmentRepository.GetAlluserBycourse(id);

            // Convertissez les entités UsersData en modèles UsersModel
            var usersModelList = usersDataList.Select(user => new UsersModel
            {
                Id = user.Id,
                Nom = user.Nom
                // Mappez d'autres propriétés si nécessaire
            });

            return usersModelList;
        }

        public Task<IEnumerable<UsersModel>> GetallUsersByCourse(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UsersModel>> GetAlluserBycourse(int id)
        {
            // Appel au repository pour obtenir les entités UsersData
            IEnumerable<UsersData> usersData = await _studentEnrollmentRepository.GetAlluserBycourse(id);

            // Mappage des entités UsersData en modèles UsersModel
            var usersModels = usersData.Select(user => user.ToUserBLL());

            return usersModels;
        }
    }
}
