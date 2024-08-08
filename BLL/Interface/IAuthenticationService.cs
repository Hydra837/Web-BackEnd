using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IAuthenticationService
    {
        Task RegisterUserAsync(UsersModel users);
        Task<string> LoginAsync(string username, string password);
        string RefreshToken(string token);
        Task<string> GetUserRoleAsync(string username);
    }
}
