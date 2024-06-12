using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Portal.Data.Models;

namespace Task_Portal.Services.Task
{
    public interface ITaskService
    {
        Task<IEnumerable<Tasks>> GetTasksAsync(TaskQueryParameters queryParameters);
        Task<Tasks> GetsTaskByIdAsync(int id);
        Task<Tasks> CreateTaskAsync(Tasks task);
        Task<Tasks> UpdateTaskAsync(int id, Tasks task);
        ValueTask DeleteTaskAsync(int id);

        ValueTask UpdateTaskStatusAsync(int id, string status, int? progress = null);
    }
}
