using BLL.Models;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IusersService : ICRUDSerrvice<UsersModel>
    {
        public void Login(string username, string password);
        public void Logout();
        public void UserRole(string role);
        public void Register(string username, string password);
        Task<IEnumerable<UserCourseDetailsModel>> GetUsersCoursesAsync();
        Task DeleteAsync(int id);
        public Task<UsersModel> GetUsersByPseudo(string pseudo);
        Task<string> GetUserRoleAsync(int userId);
    }
}
