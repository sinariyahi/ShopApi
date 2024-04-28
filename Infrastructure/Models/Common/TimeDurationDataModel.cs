using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Common
{
    public class TimeDurationDataModel
    {
        public DateTime MiladiDate { get; set; }
        public string ShamsiDate { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
        public int Day { get; set; }
        public string MonthDay { get; set; }
        public Season ThreeMonthSeason { get; set; }
        public int SixMonthSeason { get; set; }
        public int ShamsiMonth { get; internal set; }
    }

    public class IndexTemplateTimeDurationDataModel
    {
        public int Value { get; set; }
        public string Text { get; set; }
        public string DateValue { get; set; }
    }
}
