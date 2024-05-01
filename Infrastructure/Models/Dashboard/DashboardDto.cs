using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Dashboard
{
    public class DashboardDto
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public int Total { get; set; }
        public string Color { get; set; }
        public string Path { get; set; }

    }
}
