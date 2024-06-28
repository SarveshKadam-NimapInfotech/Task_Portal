using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Task_Portal.Data.IRepositories;
using Task_Portal.Data.Models;
using Task_Portal.Services.IServices;

namespace Task_Portal.Services.Services
{
    public class AdminService : IAdminService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IUserService _userService;

        public AdminService(IConfiguration config, IUserRepository userRepository, IAdminRepository adminRepository, IUserService userService)
        {
            _config = config;
            _userRepository = userRepository;
            _adminRepository = adminRepository;
            _userService = userService;
        }

        public async Task<IEnumerable<Users>> GetUsersAsync(int pageNumber, int pageSize)
        {
            return await _userRepository.GetUsersAsync(pageNumber, pageSize);
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<IEnumerable<Tasks>> GetAllTasksAsync()
        {
            return await _adminRepository.GetAllTasksAsync();
        }

        public async Task ImportCategoriesAsync(Stream fileStream)
        {
            using var package = new ExcelPackage(fileStream);
            var worksheet = package.Workbook.Worksheets[0];

            var categories = new List<Category>();
            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var name = worksheet.Cells[row, 1].Text;
                categories.Add(new Category { Name = name });
            }

            await _adminRepository.BulkInsertCategoriesAsync(categories);
        }

        public async Task ImportUsersAsync(Stream fileStream)
        {
            using var package = new ExcelPackage(fileStream);
            var worksheet = package.Workbook.Worksheets[0];

            var users = new List<Users>();
            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var username = worksheet.Cells[row, 1].Text;
                var email = worksheet.Cells[row, 2].Text;
                var password = worksheet.Cells[row, 3].Text;
                var dob = DateTime.Parse(worksheet.Cells[row, 4].Text);
                var country = worksheet.Cells[row, 5].Text;
                var state = worksheet.Cells[row, 6].Text;
                var city = worksheet.Cells[row, 7].Text;

                users.Add(new Users
                {
                    Username = username,
                    Email = email,
                    Password = _userService.HashPassword(password),
                    DateOfBirth = dob,
                    Country = country,
                    State = state,
                    City = city
                });
            }

            await _userRepository.BulkInsertAsync(users);
        }

        public async Task<byte[]> ExportUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Users");

            worksheet.Cells[1, 1].Value = "Username";
            worksheet.Cells[1, 2].Value = "Email";
            worksheet.Cells[1, 3].Value = "Date of Birth";
            worksheet.Cells[1, 4].Value = "Country";
            worksheet.Cells[1, 5].Value = "State";
            worksheet.Cells[1, 6].Value = "City";

            var row = 2;
            foreach (var user in users)
            {
                worksheet.Cells[row, 1].Value = user.Username;
                worksheet.Cells[row, 2].Value = user.Email;
                worksheet.Cells[row, 3].Value = user.DateOfBirth.ToShortDateString();
                worksheet.Cells[row, 4].Value = user.Country;
                worksheet.Cells[row, 5].Value = user.State;
                worksheet.Cells[row, 6].Value = user.City;
                row++;
            }

            return package.GetAsByteArray();
        }

        public async Task<byte[]> ExportTasksAsync()
        {
            var tasks = await _adminRepository.GetAllTasksAsync();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Tasks");

            worksheet.Cells[1, 1].Value = "Title";
            worksheet.Cells[1, 2].Value = "Description";
            worksheet.Cells[1, 3].Value = "Due Date";
            worksheet.Cells[1, 4].Value = "Status";

            var row = 2;
            foreach (var task in tasks)
            {
                worksheet.Cells[row, 1].Value = task.Name;
                worksheet.Cells[row, 2].Value = task.Description;
                worksheet.Cells[row, 3].Value = task.DueDate.ToShortDateString();
                worksheet.Cells[row, 4].Value = task.Status;
                row++;
            }

            return package.GetAsByteArray();
        }

        public async Task CreateCategoryAsync(Category category)
        {
            await _adminRepository.AddCategoryAsync(category);
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _adminRepository.GetAllCategoriesAsync();
        }

        public async Task UpdateCategoryAsync(int id, Category category)
        {
            var existingCategory = await _adminRepository.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                throw new Exception("Category not found");
            }

            existingCategory.Name = category.Name;
            await _adminRepository.UpdateCategoryAsync(existingCategory);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _adminRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            await _adminRepository.DeleteCategoryAsync(category);
        }

        public async Task AssignAdminRoleAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId.ToString());
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var adminRole = await _userRepository.GetRoleByNameAsync("Admin");
            if (adminRole == null)
                throw new KeyNotFoundException("Admin role not found");

            var userRole = new UserRole { UserId = userId, RoleId = adminRole.Id };
            await _adminRepository.AddUserRoleAsync(userRole);
        }
    }
}
