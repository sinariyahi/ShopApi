using Microsoft.VisualBasic.FileIO;
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
    [Table("SliderAttachments", Schema = "Media")]
    public class SliderAttachment
    {
        [Key]
        public Guid Id { get; set; }
        public int SliderId { get; set; }
        public virtual Slider Slider { get; set; }

       public Device Device { get; set; }
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
