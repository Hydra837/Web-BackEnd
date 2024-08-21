using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IStudent_Management  : IcrudRepository
    {
        public IEnumerable<CoursData> GetAllCourseByUser(int id);
        public Task InsertUserCoursAsync(int id, int id_cours);
        public Task DeleteAsync1(int id, int course);
        public Task CreateAsync(UsersData usersData);
        public Task InsertStudentCoursesAsync(int user, int course);
        public Task Deleteteacher(int idteacher, int course);
        public Task UpdateTeacherToCourse(int teacherId, int courseId);
        public Task<IEnumerable<UserAssignementsData>> GetuserResult(int id);
        public Task<List<UserAssignementsData>> GetAllUsersAssignmentsGradesForCourse(int courseId);

        Task SaveChangesAsync();


    }
}
