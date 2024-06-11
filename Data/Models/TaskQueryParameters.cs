using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Portal.Data.Models
{
    public class TaskQueryParameters
    {
        public string Status { get; set; }
        public DateTime? DueDate { get; set; }
        public string AssignedTo { get; set; }
        public string SortBy { get; set; } = "DueDate";
        public string SortOrder { get; set; } = "asc"; // or "desc"
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
