using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Portal.Data.Models;

namespace Task_Portal.Services.IServices
{
    public interface IAdminService
    {
        //Task<string> LoginAsync(string email, string password);
        Task<IEnumerable<Users>> GetUsersAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Users>> GetAllUsersAsync();
        Task<IEnumerable<Tasks>> GetAllTasksAsync();
        Task ImportCategoriesAsync(Stream fileStream);
        Task ImportUsersAsync(Stream fileStream);
        Task CreateCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task UpdateCategoryAsync(int id, Category category);
        Task DeleteCategoryAsync(int id);
        Task<byte[]> ExportTasksAsync();
        Task<byte[]> ExportUsersAsync();
        Task AssignAdminRoleAsync(int userId);
    }
}
