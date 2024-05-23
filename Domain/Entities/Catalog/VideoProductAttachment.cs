using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Common;

namespace Domain.Entities.Catalog
{

    [Table("VideoProductAttachments", Schema = "Catalog")]
    public class VideoProductAttachment
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public string VideoTitle { get; set; }
        public string VideoLink { get; set; }
        public virtual ICollection<VideoFileProductAttachment> VideoFileProductAttachments { get; set; }

        public VideoProductAttachment()
        {
            VideoFileProductAttachments = new HashSet<VideoFileProductAttachment>();
        }
    }

    [Table("VideoFileProductAttachments", Schema = "Catalog")]
    public class VideoFileProductAttachment
    {
        [Key]
        public Guid Id { get; set; }
        public int VideoProductAttachmentId { get; set; }
        public virtual VideoProductAttachment VideoProductAttachment { get; set; }

        [MaxLength(128)]
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileContentType { get; set; }
        public string FileSize { get; set; }
        public FileType FileType { get; set; }
        public string FilePath { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
