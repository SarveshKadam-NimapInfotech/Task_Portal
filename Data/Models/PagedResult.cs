﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Portal.Data.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        //public int TotalCount { get; set; }
        //public int PageNumber { get; set; }
        //public int PageSize { get; set; }
    }
}
