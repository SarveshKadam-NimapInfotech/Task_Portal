using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Portal.Data.Models;

namespace Task_Portal.Data.IRepositories
{
    public interface IReminderRepository
    {
        Task<Reminder> CreateReminderAsync(Reminder reminder);
        Task<List<Reminder>> GetDueRemindersAsync();
        Task MarkAsSentAsync(Reminder reminder);
    }
}
