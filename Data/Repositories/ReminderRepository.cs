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
    public class ReminderRepository : IReminderRepository
    {
        private readonly ApplicationDbContext _context;

        public ReminderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Reminder> CreateReminderAsync(Reminder reminder)
        {
            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync();
            return reminder;
        }

        public async Task<List<Reminder>> GetDueRemindersAsync()
        {
            var now = DateTime.Now;
            return await _context.Reminders
                .Include(r => r.Task)
                .Where(r => r.ReminderTime <= now && !r.HasBeenSent) // Only get reminders that haven't been sent
                .ToListAsync();
        }

        public async Task MarkAsSentAsync(Reminder reminder)
        {
            reminder.HasBeenSent = true;
            _context.Reminders.Update(reminder);
            await _context.SaveChangesAsync();
        }

    }
}
