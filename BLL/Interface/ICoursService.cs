using BLL.Models;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL.Interface
{
    public interface ICoursService : ICRUDSerrvice<CoursModel>
    {
        //public IEnumerable<CoursModel> GetAllAvailble();
        Task<IEnumerable<CoursModel>> GetAllAvailble();
       // Task InsertUserCourseAsync(int id, int id_cours);
        Task<IEnumerable<CoursModel>> GetAvailableCoursesAsync();
       // Task EnrollUserInCourseAsync(CoursModel cours, UsersModel users );
        Task<IEnumerable<CoursModel>> GetAllAsync();
        // public Task UpdateAsync(int id, CoursModel cours);
        //   public Task<IEnumerable<CoursModel>> GetallByUser(int id);
        // Task UpdateAsync(int id, CoursModel cours);
        public Task<IEnumerable<CoursModel>> GetAllByTeacher(int id);

    }
}
