using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Portal.Data.IRepositories;
using Task_Portal.Data.Models;
using Task_Portal.Services.IServices;

namespace Task_Portal.Services.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService; // Add an email service
        private readonly ICategoryRepository _categoryRepository;

        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository, IEmailService emailService, ICategoryRepository categoryRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _categoryRepository = categoryRepository;
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
            existingTask.CreatedByUserId = task.CreatedByUserId;
            existingTask.Status = task.Status;
            existingTask.Progress = task.Progress;

            await _taskRepository.UpdateTaskAsync(existingTask);
            return existingTask; // Return the updated task
        }

        public async Task DeleteTaskAsync(int id)
        {
            await _taskRepository.DeleteTaskAsync(id);
        }

        public async Task UpdateTaskStatusAsync(int id, string status, int? progress = null)
        {
            await _taskRepository.UpdateTaskStatusAsync(id, status, progress);
        }

        public async Task AssignTaskAsync(int taskId, string userId)
        {
            var userIdOrEmail = await _userRepository.GetEmailbyUserId(userId);
            await _taskRepository.AssignTaskAsync(taskId, userIdOrEmail);
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            await _emailService.SendEmailAsync(userIdOrEmail, "Task Assigned", $"You have been assigned the task: {task.Name}");
        }

        public async Task UpdateTaskAcceptanceAsync(int taskId, bool isAccepted)
        {
            await _taskRepository.UpdateTaskAcceptanceAsync(taskId, isAccepted);
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            string statusMessage = isAccepted ? "accepted" : "declined";
            await _emailService.SendEmailAsync(task.AssignedToUserId, "Task Acceptance", $"The task: {task.Name} has been {statusMessage} by the assigned user.");
        }

        public Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return _taskRepository.GetCategoriesAsync();
        }

        public Task<Category> GetCategoryByIdAsync(int id)
        {
            return _taskRepository.GetCategoryByIdAsync(id);
        }

        public Task<IEnumerable<Tasks>> GetTasksByCategoryAsync(int categoryId, TaskQueryParameters queryParameters)
        {
            return _taskRepository.GetTasksByCategoryAsync(categoryId, queryParameters);
        }

        public async Task<IEnumerable<Tasks>> GetTasksByCategoryAsync(int categoryId)
        {
            return await _taskRepository.GetTasksByCategoryAsync(categoryId);
        }

        public async Task<bool> AssignCategoryAsync(int taskId, int categoryId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                return false;
            }

            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                return false;
            }

            task.CategoryId = categoryId;
            await _taskRepository.UpdateTaskAsync(task);

            return true;
        }
    }
}
