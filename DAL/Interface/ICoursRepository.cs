using DAL.Data;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface ICoursRepository : IefCrudRepository<CoursData>
    {
        public Task<IEnumerable<CoursData>> GetAllAvailable();
        public IEnumerable<CoursData> GetAllAvailble1();
       // Task InsertUserCourseAsync(CoursData cours, UsersData user);
        Task<IEnumerable<CoursData>> Getall();
        public Task<IEnumerable<CoursData>> GetallByUser(int id);
        public Task<IEnumerable<UsersData>> GetAllUuserBycourse(int id);
        public Task<IEnumerable<CoursData>> GetAllCourseByTeacher(int id);
    }
}
