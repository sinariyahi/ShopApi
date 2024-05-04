using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Media
{
    public class VideoCategoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveTitle { get; set; }
        public int? SortOrder { get; set; }
        public string Remark { get; set; }
    }


}
