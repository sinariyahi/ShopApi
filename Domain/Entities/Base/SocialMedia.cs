using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Base
{
    [Table("SocialMedias", Schema = "Base")]
    public class SocialMedia
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string Link { get; set; }
//        public virtual ICollection<SocialMediaAttachment> SocialMediaAttachments { get; set; }
        public SocialMedia()
        {
 //           SocialMediaAttachments = new HashSet<SocialMediaAttachment>();
        }

    }


}
