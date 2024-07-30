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
        public IEnumerable<CoursModel> CoursPourchaqueEtu();
        public Task InsertStudentCourseAsync(UsersModel user, CoursModel course);
        public Task InsertStudentCourseAsync2(int user, int course);
        public Task<IEnumerable<UsersModel>> GetAlluserBycourse(int id);

        // Changer Cours Model en Cours Etu pour avoir les noms étudiant aussi
    }
}
