using BLL.Models;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IStudentManagmentService : ICRUDSerrvice<CoursModel>
    {
        public IEnumerable<CoursModel> GetAllCourseByUser(int id);
        public Task InsertUserCoursAsync(int id, int id_cours);
        public Task DeleteAsync1(int id, int coursid);
        // public Task InsertStudentCoursesAsync(int user, int course);
        public Task Deleteteacher(int idteacher, int course);
        public Task UpdateTeacherToCourse(int teacherId, int courseId);
        public Task<UsersModel>GetTeacherName(int teacherId);
        public Task<IEnumerable<UserAssignementsModel>> GetuserResult(int id);
    }
}
