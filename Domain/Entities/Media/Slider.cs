using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Media
{
    [Table("Sliders", Schema = "Media")]
    public class Slider
    {
        public int Id { get; set; }
        [Required, MaxLength(128)]
        public string Title { get; set; }
        [MaxLength(512)]
        public string Abstract { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public int? SortOrder { get; set; }
        public string SeoTitle { get; set; }
        public string Link { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }



        public virtual ICollection<SliderAttachment> SliderAttachments { get; set; }

        public Slider()
        {
            SliderAttachments = new HashSet<SliderAttachment>();

        }
    }
}
