using BLL.Models;
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
        public Task DeleteAsync(int id);
    }
}
