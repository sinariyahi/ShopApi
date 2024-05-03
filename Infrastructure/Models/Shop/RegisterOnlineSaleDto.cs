using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Shop
{
    public class RegisterOnlineSaleDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string NationalCode { get; set; }
        public string HomeTel { get; set; }
        public string OfficeTel { get; set; }
        public string Remark { get; set; }
        public string PaymentDate { get; set; }
        public int ParishId { get; set; }
        public long Price { get; set; }
        public long DisCount { get; set; }
        public long LaborAmount { get; set; }
        public int TraceNo { get; set; }
        public List<RegisterOnlineSaleRowDto> PartList { get; set; } = new List<RegisterOnlineSaleRowDto>();

    }

    public class RegisterOnlineSaleRowDto
    {
        public string PartNo { get; set; }
        public int Qty { get; set; }
    }
}
