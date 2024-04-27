using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.ChartData
{
    public class ChartDataDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Value { get; set; }
        public string Category { get; set; }
    }

    public class ChartColumnInfo
    {
        public string Title { get; set; }
        public string ColumnName { get; set; }
    }
}
