using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Common
{
    public class TotalKeyValueDataModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public bool IsOrganizationUnit { get; set; } = false;
    }
}
