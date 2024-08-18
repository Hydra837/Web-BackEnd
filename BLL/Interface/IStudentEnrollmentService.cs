using BLL.Models;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IStudentEnrollmentService
    {
        public void InsertStudentCourse(int id, int id_Course);
        Task<IEnumerable<CoursModel>> CoursPourchaqueEtuAsync(int id);
        public Task InsertStudentCourseAsync(UsersModel user, CoursModel course);
        public Task InsertStudentCourseAsync2(int user, int course);
        public Task<IEnumerable<UsersModel>> GetAlluserBycourse(int id);
        public Task<IEnumerable<CoursModel>> EnrolledStudentAsync(int id);
        Task<IEnumerable<Student_EnrollmentModel>> GetAllGradesAsync();
        Task<Student_EnrollmentModel> GetByUserIdAsync(int userId);
        Task<Student_EnrollmentModel> GetByCourseAsync(int courseId);
        //   Task<IEnumerable<GradeData>> GetByCoursesAsync(int id);
        Task InsertGrade(int id, int grade);
        Task DeleteAsync(int id);
        Task UpdateGrade(Student_EnrollmentModel student);
        public  Task<bool> UpdateGrade(int id, int grade);
       // public Task<bool> UpdateGrade(int iduser, int coursid, int grade);
        Task<bool> UpdateGradesAsync(int userId, int courseId, int grade);
        Task<IEnumerable<UsersModel>> GetUsersWithCoursesAsync();
        Task<ICollection<UsersModel>> GetStudentAllCourseAsync();
        Task<ICollection<UsersModel>> GetTeacherAllCourseAsync();
        public Task<CoursModel> GetCoursWithUsersAsync1(int coursId);
        public Task<UsersModel> GetUserWithCoursesAsync1(int userId);

        public Task<List<UsersModel>> GetAllStudentsWithCoursesAsync();
        public Task<List<UsersModel>> GetAllProfessorsWithCoursesAsync();
        public Task<UsersModel> GetProfessorWithCoursesAsync(int professorId);
        Task<List<UsersModel>> GetUsersWithCoursesAssignmentsAndGradesAsync();


    }

    // Changer Cours Model en Cours Etu pour avoir les noms étudiant aussi
}

