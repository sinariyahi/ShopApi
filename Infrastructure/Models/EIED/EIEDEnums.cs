using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.EIED
{
    public class EIEDEnums
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
    public class EIEDListEnums
    {
        public List<EIEDEnums> DashboardLevels { get; set; }
        public List<EIEDEnums> MeasurementPeriods { get; set; }
        public List<EIEDEnums> ShowTrends { get; set; }
        public List<EIEDEnums> ShowTypes { get; set; }
        public List<EIEDEnums> UnitTypes { get; set; }
        public List<EIEDEnums> IndexTypes { get; set; }
        public List<EIEDEnums> ActionTypes { get; set; }

    }

}
