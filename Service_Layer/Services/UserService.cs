using Task_Portal.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Task_Portal.Data.IRepositories;
using Task_Portal.Services.IServices;
using Task_Portal.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Task_Portal.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        //private readonly UserManager<Users> _userManager;
        //private readonly List<string> _blacklistedTokens = new List<string>();

        public UserService(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
            //_userManager = userManager;
        }

        public async Task<Users> RegisterUserAsync(string username, string email, string password, DateTime dob, string country, string state, string city)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(username);
            if (existingUser != null)
            {
                throw new Exception("User already exists");
            }

            var user = new Users
            {
                Username = username,
                Password = HashPassword(password),
                Email = email,
                DateOfBirth = dob,
                Country = country,
                State = state,
                City = city,

                UserRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        //RoleId = _context.Roles.Single(r => r.RoleName == "User").RoleId
                        RoleId = await _userRepository.GetRoleId("User")
                    }
                }
            };

            await _userRepository.AddUserAsync(user);
            return user;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null || !VerifyPassword(password, user.Password))
            {
                return null;
            }

            return await GenerateToken(user);
        }

        public async Task<string> LogoutAsync(IEnumerable<Claim> claims)
        {
            // Here you can perform any additional logout logic if needed
            var shortLivedToken = GenerateShortLivedToken(claims);
            return shortLivedToken;
        }



        public async Task<bool> ResetPasswordAsync(string username, string newPassword)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return false;
            }

            user.Password = HashPassword(newPassword);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        //public async Task<List<Users>> GetAllUsersAsync()
        //{
        //    return await _userRepository.GetAllUsersAsync();
        //}
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            return HashPassword(password) == passwordHash;
        }

        private async Task<string> GenerateToken(Users user, int expiryInSeconds = 1800)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique identifier for the token
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString())
            };


            var roles = await _userRepository.Roles(user.Id);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));

            }
            var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddSeconds(expiryInSeconds),
            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //        new Claim(ClaimTypes.Name, user.Username),
            //        new Claim(ClaimTypes.Email, user.Email),
            //    }),
            //    Expires = DateTime.UtcNow.AddSeconds(expiryInSeconds),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //return tokenHandler.WriteToken(token);
        }

        public string GenerateShortLivedToken(IEnumerable<Claim> claims, int expiryInSeconds = 3)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(expiryInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<Users> GetUserByIdAsync(string userId)
        {
           return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<Users> UpdateUserAsync(string userId, Users user)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.Email = user.Email;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.Country = user.Country;
            existingUser.State = user.State;
            existingUser.City = user.City;

            await _userRepository.UpdateUserAsync(existingUser);
            return existingUser;
        }

        public async Task<IEnumerable<Users>> GetUsersAsync(int pageNumber, int pageSize)
        {
            return await _userRepository.GetUsersAsync(pageNumber, pageSize);
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task InvalidateToken(string token)
        {
            var blacklistedToken = new BlacklistedToken
            {
                Token = token,
                BlacklistedAt = DateTime.UtcNow
            };

            await _userRepository.InvalidateTokenAsync(blacklistedToken);
            
        }

        public bool IsTokenValid(string token)
        {
            bool isValid = _userRepository.IsTokenValidAsync(token);
            return isValid;
           
        }
    }
}
