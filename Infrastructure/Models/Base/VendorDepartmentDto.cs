using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Base
{
    public class VendorDepartmentDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsActive { get; set; }
        public string IsActiveTitle { get; set; }
    }
}
