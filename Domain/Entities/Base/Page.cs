using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Base
{
    [Table("Pages", Schema = "Base")]
    public class Page
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(256)]
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string Link { get; set; }

        public int SortOrder { get; set; }
//        public PagesLinkType PagesLinkType { get; set; }

    }

}
