using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Portal.Data.IRepositories;
using Task_Portal.Data.Models;

namespace Task_Portal.Data.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tasks>> GetAllTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task BulkInsertCategoriesAsync(List<Category> categories)
        {
            await _context.Categories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserRoleAsync(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
        }
    }
}
