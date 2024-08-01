using DAL.Data;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IStudent_EnrollmentRepository
    {
        Task InsertStudentCourseAsync(UsersData user, CoursData course);
        Task<IEnumerable<CoursData>> CoursPourchaqueEtuAsync(UsersData user);
        public Task InsertStudentCourseAsync2(int user, int course);
        public Task<IEnumerable<UsersData>> GetAlluserBycourse(int id);
        public Task<IEnumerable<CoursData>> EnrolledStudent(int id);
        Task<IEnumerable<Student_EnrollementData>> GetAllGradesAsync();
        Task<Student_EnrollementData> GetByUserIdAsync(int userId);
        Task<Student_EnrollementData> GetByCourseAsync(int courseId);
     //   Task<IEnumerable<GradeData>> GetByCoursesAsync(int id);
        Task InsertGrade(int id, int grade);
        Task DeleteAsync(int id);
        Task UpdateGrade(Student_EnrollementData student);
    }
}
