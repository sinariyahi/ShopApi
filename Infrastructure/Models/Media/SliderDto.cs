using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Media
{
    public class SliderDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public int? SortOrder { get; set; }
        public string SeoTitle { get; set; }
        public string Link { get; set; }
        public string IsActiveTitle { get; set; }
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public List<IFormFile> MobileImages { get; set; }
        public List<FileItemDto> MobileImagesAttachment { get; set; }
        public List<IFormFile> DesktopImages { get; set; }
        public List<FileItemDto> DesktopAttachment { get; set; }

    }


}
