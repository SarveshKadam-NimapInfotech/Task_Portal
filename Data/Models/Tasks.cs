using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Portal.Data.Models
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public string Priority { get; set; }
        public string AssignedToUserId { get; set; }
        public string CreatedByUserId { get; set; }

        [Required]
        public string Status { get; set; } // Status of the task (e.g., "InProgress", "Completed", "Cancelled", "PendingAcceptance")

        public int Progress { get; set; } // Progress of the task in percentage (0-100)

        public bool IsAccepted { get; set; } // Indicates if the task is accepted by the assigned user

        public int CategoryId { get; set; } // New field for category association
        public Category Category { get; set; } // Navigation property
    }
}
