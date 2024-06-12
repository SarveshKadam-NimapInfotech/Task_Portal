using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Portal.Data.Models
{
    public class UpdateTaskStatusRequest
    {
        public string Status { get; set; }
        public int? Progress { get; set; }
    }
}
