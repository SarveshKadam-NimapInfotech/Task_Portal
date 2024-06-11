using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Portal.Data.Models;

namespace Task_Portal.Data.Repositories.TaskRepo
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Tasks>> GetTasksAsync(TaskQueryParameters queryParameters);
        Task<Tasks> GetTaskByIdAsync(int id);
        Task AddTaskAsync(Tasks task);
        Task UpdateTaskAsync(Tasks task);
        Task DeleteTaskAsync(int id);
    }
}
