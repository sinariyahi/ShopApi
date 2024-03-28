using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class ApplicationConfigDto
    {
        public int Id { get; set; }
        public string ApplicationTitle { get; set; }
        public string ApplicationTitleEn { get; set; }
        public string MainLogoBase64 { get; set; }
        public string MainLogoBase64En { get; set; }
        public IFormFile MainLogo { get; set; }
        public IFormFile MainLogoEn { get; set; }
        public string SidebarLogoBase64 { get; set; }
        public string SidebarLogoBase64En { get; set; }
        public IFormFile SidebarLogo { get; set; }
        public IFormFile SidebarLogoEn { get; set; }
        public bool? ShowFooterEn { get; set; } = false;
        public bool ShowFooter { get; set; } = false;
        public string FooterEn { get; set; }
        public string Footer { get; set; }
        public int CurrentYear { get; set; }
    }
}
