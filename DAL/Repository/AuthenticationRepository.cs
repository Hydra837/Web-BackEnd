using DAL.Data;
using DAL.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly EFDbContextData _context;
        

        public AuthenticationRepository(EFDbContextData context)
        {
            _context = context;
        }

        public async Task<UsersData> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Pseudo == username);
        }

        public async Task AddUserAsync(UsersData user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetUserRoleAsync(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Pseudo == username);
            return user?.Roles;
        }

        public Task<RefreshTokenData> GetRefreshTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task SaveRefreshTokenAsync(RefreshTokenData refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        //public async Task<RefreshToken> GetRefreshTokenAsync(string token)
        //{
        //    return await _context.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == token);
        //}

        //public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        //{
        //    await _context.RefreshTokens.AddAsync(refreshToken);
        //    await _context.SaveChangesAsync();
        //}
    }
}
