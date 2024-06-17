using Task_Portal.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Portal.Data.Repositories.UserRepo
{
    public interface IUserRepository
    {
        Task<Users> GetUserByUsernameAsync(string username);
        Task<Users> GetUserByEmailAsync(string email);
        // Task<List<Users>> GetAllUsersAsync();

        Task<string> GetEmailbyUserId(string userIdOrEmail);
        Task AddUserAsync(Users user);
        Task SaveChangesAsync();
    }
}
