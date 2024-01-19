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
    [Table("Brands", Schema = "Catalog")]
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string Title { get; set; }

        [MaxLength(256)]
        public string EnTitle { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }

        public int? SortOrder { get; set; }
        public virtual ICollection<BrandAttachment> BrandAttachments { get; set; }
        //public virtual ICollection<Product> Products { get; set; }

        public Brand()
        {
         //   BrandAttachments = new HashSet<BrandAttachment>();
       //     Products = new HashSet<Product>();
        }
    }

    [Table("BrandAttachments", Schema = "Catalog")]
    public class BrandAttachment
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(128)]
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileContentType { get; set; }
        public string FileSize { get; set; }
     //   public FileType FileType { get; set; }
        public string FilePath { get; set; }
        public DateTime CreateDate { get; set; }
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; }

    }
}
//sina
