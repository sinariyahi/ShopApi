using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Catalog
{
    [Table("CategoryAttachments", Schema = "Catalog")]
    public class CategoryAttachment
    {
        [Key]
        public Guid Id { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [MaxLength(128)]
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileContentType { get; set; }
        public string FileSize { get; set; }
     //   public FileType FileType { get; set; }
        public string FilePath { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
