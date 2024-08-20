using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IAuthenticationRepository
    {
        Task<UsersData> GetUserByUsernameAsync(string username);
        Task AddUserAsync(UsersData user);
        Task<string> GetUserRoleAsync(string username);
        // Méthodes pour gérer les tokens de rafraîchissement, si applicable
        Task<RefreshTokenData> GetRefreshTokenAsync(string token);
        Task SaveRefreshTokenAsync(RefreshTokenData refreshToken);
        Task SaveChangesAsync();
    }
}
