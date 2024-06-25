using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Portal.Data.Models;

namespace Task_Portal.Services.IServices
{
    public interface ITaskService
    {
        Task<IEnumerable<Tasks>> GetTasksAsync(TaskQueryParameters queryParameters);
        Task<Tasks> GetsTaskByIdAsync(int id);
        Task<Tasks> CreateTaskAsync(Tasks task);
        Task<Tasks> UpdateTaskAsync(int id, Tasks task);
        System.Threading.Tasks.Task DeleteTaskAsync(int id);

        System.Threading.Tasks.Task UpdateTaskStatusAsync(int id, string status, int? progress = null);
        System.Threading.Tasks.Task AssignTaskAsync(int taskId, string userId); // method for assigning tasks
        System.Threading.Tasks.Task UpdateTaskAcceptanceAsync(int taskId, bool isAccepted); // method for updating acceptance status
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task<IEnumerable<Tasks>> GetTasksByCategoryAsync(int categoryId, TaskQueryParameters queryParameters);
        Task<IEnumerable<Tasks>> GetTasksByCategoryAsync(int categoryId);
        Task<bool> AssignCategoryAsync(int taskId, int categoryId);
    }
}
