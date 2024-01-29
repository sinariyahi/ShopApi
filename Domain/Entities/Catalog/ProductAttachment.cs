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
    [Table("ProductAttachment", Schema = "Catalog")]
    public class ProductAttachment
    {
        [Key]
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

       // public ProductAttachmentType ProductAttachmentType { get; set; }

        [MaxLength(128)]
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileContentType { get; set; }
        public string FileSize { get; set; }
        //public FileType FileType { get; set; }
        public string FilePath { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
