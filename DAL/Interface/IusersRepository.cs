using DAL.Data;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IusersRepository : IefCrudRepository<UsersData>
    {
        public void Login (string username, string password);
        public void Logout ();
        public void UserRole(string role);
        public void Register (string username, string password);
        public Task<IEnumerable<UserCourseDetailsData>> GetUsersCoursesAsync();
        public Task<IEnumerable<CoursData>> GetallByUser(int id);
        public Task<UsersData> GetUsersByPseudo(string pseudo);
        Task<string> GetUserRoleByIdAsync(int userId);


    }
}
