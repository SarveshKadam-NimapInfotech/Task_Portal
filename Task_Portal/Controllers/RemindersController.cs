using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_Portal.Data.IRepositories;
using Task_Portal.Data.Models;
using Task_Portal.Services.IServices;

namespace Task_Portal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RemindersController : ControllerBase
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public RemindersController(IReminderRepository reminderRepository, ITaskRepository taskRepository, IUserRepository userRepository, IEmailService emailService)
        {
            _reminderRepository = reminderRepository;
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        [HttpPost("schedule")]
        public async Task<IActionResult> ScheduleReminder([FromBody] ScheduleReminderRequest request)
        {
            if (request.ReminderTime <= DateTime.UtcNow)
            {
                return BadRequest("Reminder time must be in the future.");
            }

            var task = await _taskRepository.GetTaskByIdAsync(request.TaskId);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            var user = await _userRepository.GetUserByIdAsync(task.AssignedToUserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var reminder = new Reminder
            {
                TaskId = request.TaskId,
                ReminderTime = request.ReminderTime,
                Email = user.Email
            };

            var createdReminder = await _reminderRepository.CreateReminderAsync(reminder);
            return Ok(createdReminder);
        }
    }
}
