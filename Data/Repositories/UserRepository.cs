using Task_Portal.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Portal.Data.IRepositories;

namespace Task_Portal.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Users> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<Users> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        //public async Task<List<Users>> GetAllUsersAsync()
        //{
        //    return await _context.Users.ToListAsync();
        //}

        public async Task AddUserAsync(Users user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetEmailbyUserId(string userIdOrEmail)
        {

            if (userIdOrEmail.Contains(".com"))
            {
                return userIdOrEmail;
            }
            else
            {
                int userId = Convert.ToInt32(userIdOrEmail);
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                var userEmail = user?.Email;

                return userEmail;
            }

        }

        public async Task<Users> GetUserByIdAsync(string userId)
        {
            int user = Convert.ToInt32(userId);
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == user);
        }

        public async Task UpdateUserAsync(Users user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Role>> Roles(int userId)
        {
            var userRoles = await _context.UserRoles.Where(u => u.UserId == userId).ToListAsync();
            var roleIds = userRoles.Select(u => u.RoleId).ToList();
            var roles =await _context.Roles.Where(r => roleIds.Contains(r.Id)).ToListAsync();
            return roles;
        }

        public async Task<IEnumerable<Users>> GetUsersAsync(int pageNumber, int pageSize)
        {
            return await _context.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task BulkInsertAsync(List<Users> users)
        {
            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetRoleId(string role)
        {
            var data = await _context.Roles.SingleAsync(r => r.Name == role);
            return data.Id;
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public async Task InvalidateTokenAsync(BlacklistedToken blacklistedToken)
        {
            _context.BlacklistedTokens.Add(blacklistedToken);
            await _context.SaveChangesAsync();
        }

        public bool IsTokenValidAsync(string token)
        {
            return !_context.BlacklistedTokens.Any(t => t.Token == token);
        }
    }
}
