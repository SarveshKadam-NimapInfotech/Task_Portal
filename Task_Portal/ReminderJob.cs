using Task_Portal.Data.IRepositories;
using Task_Portal.Services.IServices;

namespace Task_Portal.API
{
    public class ReminderJob
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly IEmailService _emailService;

        public ReminderJob(IReminderRepository reminderRepository, IEmailService emailService)
        {
            _reminderRepository = reminderRepository;
            _emailService = emailService;
        }

        public async Task Execute()
        {
            var dueReminders = await _reminderRepository.GetDueRemindersAsync();

            foreach (var reminder in dueReminders)
            {
                if (reminder.Task != null)
                {
                    await _emailService.SendEmailAsync(reminder.Email, "Task Reminder", $"You have a reminder for task {reminder.Task.Name}.");
                    await _reminderRepository.MarkAsSentAsync(reminder); // Mark the reminder as sent
                }
            }
        }
    }
}