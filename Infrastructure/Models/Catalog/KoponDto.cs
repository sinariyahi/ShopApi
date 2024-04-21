using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Catalog
{
    public class KoponDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveTitle { get; set; }
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public int Percent { get; set; }
        public string Remark { get; set; }
        public string Code { get; set; }

    }

    public class UserKoponDto
    {
        public int Percent { get; set; }
        public string Code { get; set; }

    }

}
