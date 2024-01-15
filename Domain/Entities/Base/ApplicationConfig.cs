using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Base
{
    [Table("ApplicationConfigs", Schema = "Base")]
    public class ApplicationConfig
    {
        public int Id { get; set; }
        [MaxLength(256)]
        public string ApplicationTitle { get; set; }
        [MaxLength(256)]
        public string ApplicationTitleEn { get; set; }
        public byte[] MainLogo { get; set; }
        public byte[] MainLogoEn { get; set; }
        public byte[] SidebarLogo { get; set; }
        public byte[] SidebarLogoEn { get; set; }
        public bool? ShowFooterEn { get; set; } = false;
        public bool ShowFooter { get; set; } = false;
        [MaxLength(256)]
        public string Footer { get; set; }
        [MaxLength(256)]
        public string FooterEn { get; set; }
    }
}
