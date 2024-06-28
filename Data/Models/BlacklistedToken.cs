using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Portal.Data.Models
{
    public class BlacklistedToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime BlacklistedAt { get; set; }
    }
}
