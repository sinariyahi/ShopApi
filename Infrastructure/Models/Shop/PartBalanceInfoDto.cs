using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Shop
{
    public class PartBalanceInfoDto
    {
        public string PartNo { get; set; }
        public int? LegalAvail { get; set; }
        public long? CustomerPrice { get; set; }
    }
}
