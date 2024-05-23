using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Common;

namespace Domain.Entities.Media
{
    [Table("Videos", Schema = "Media")]
    public class Video
    {
        public int Id { get; set; }
        [Required, MaxLength(128)]
        public string Title { get; set; }
        [MaxLength(512)]
        public string Remark { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public int? SortOrder { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string VideoLink { get; set; }
        public VideoSource VideoSource { get; set; }
        public int VideoCategoryId { get; set; }
        public string ShortDescription { get; set; }

        public virtual VideoCategory VideoCategory { get; set; }

        public virtual ICollection<VideoAttachment> VideoAttachments { get; set; }
        public virtual ICollection<VideoCoverAttachment> VideoCoverAttachments { get; set; }

        public Video()
        {
            VideoAttachments = new HashSet<VideoAttachment>();
            VideoCoverAttachments = new HashSet<VideoCoverAttachment>();

        }
    }
}
