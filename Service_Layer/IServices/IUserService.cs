﻿using Task_Portal.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Task_Portal.Services.IServices
{
    public interface IUserService
    {
        Task<Users> RegisterUserAsync(string username, string email, string password, DateTime dob, string country, string state, string city);
        Task<string> LoginAsync(string username, string password);
        Task<string> LogoutAsync(IEnumerable<Claim> claims);

        Task<bool> ResetPasswordAsync(string username, string newPassword);
        // Task<List<Users>> GetAllUsersAsync();

        Task<Users> GetUserByIdAsync(string userId);
        Task<Users> UpdateUserAsync(string userId, Users user);
        string HashPassword(string password);

        Task<IEnumerable<Users>> GetUsersAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Users>> GetAllUsersAsync();

        Task InvalidateToken(string token);

        bool IsTokenValid(string token);
    }
}
