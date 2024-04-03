using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Base
{
    public class DeliveryLocationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }
    }
}
