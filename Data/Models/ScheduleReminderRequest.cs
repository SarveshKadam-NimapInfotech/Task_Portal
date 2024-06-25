using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Portal.Data.Models
{
    public class ScheduleReminderRequest
    {
        public int TaskId { get; set; }
        public DateTime ReminderTime { get; set; }
    }
}
