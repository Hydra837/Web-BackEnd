using BLL.Models;
using DAL.Data;
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
    }

    // Changer Cours Model en Cours Etu pour avoir les noms étudiant aussi
}

