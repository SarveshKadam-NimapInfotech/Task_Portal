using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Task_Portal.Data.Models
{
    public class Reminder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        public Tasks Task { get; set; }

        [Required]
        public DateTime ReminderTime { get; set; }

        [Required]
        public string Email { get; set; }

        public bool HasBeenSent { get; set; } // New property to track if the email has been sent
    }
}