using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Portal.Data.Models;

namespace Task_Portal.Data.IRepositories
{
    public interface IAdminRepository
    {
        Task<IEnumerable<Tasks>> GetAllTasksAsync();
        Task BulkInsertCategoriesAsync(List<Category> categories);
        Task AddCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);

        Task AddUserRoleAsync(UserRole userRole);
    }
}
