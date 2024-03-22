using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class Pagination
    {
        public int PageNumber { get; set; }
        public int PageItemCount { get; set; } = 25;
        public int CurrentPage { get; set; } = 1;
        public bool WithPagination { get; set; }
        public int TotalRows { get; set; }
    }
}
