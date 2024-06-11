using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Portal.Data.Models;
using Task_Portal.Data.Repositories.TaskRepo;

namespace Task_Portal.Services.Task
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<Tasks>> GetTasksAsync(TaskQueryParameters queryParameters)
        {
            return await _taskRepository.GetTasksAsync(queryParameters);
        }

        public async Task<Tasks> GetsTaskByIdAsync(int id)
        {
            return await _taskRepository.GetTaskByIdAsync(id);
        }

        public async Task<Tasks> CreateTaskAsync(Tasks task)
        {
            await _taskRepository.AddTaskAsync(task);
            return task; // Return the created task
        }

        public async Task<Tasks> UpdateTaskAsync(int id, Tasks task)
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(id);
            if (existingTask == null)
            {
                return null;
            }

            existingTask.Name = task.Name;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;
            existingTask.Priority = task.Priority;
            existingTask.AssignedToUserId = task.AssignedToUserId;

            await _taskRepository.UpdateTaskAsync(existingTask);
            return existingTask; // Return the updated task
        }

        public async ValueTask DeleteTaskAsync(int id)
        {
            await _taskRepository.DeleteTaskAsync(id);
        }
    }
}
