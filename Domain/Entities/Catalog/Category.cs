using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Catalog
{
    [Table("Categories", Schema = "Catalog")]
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(128), Required]
        public string CategoryName { get; set; }
        public string EnName { get; set; }

        public bool IsActive { get; set; }
        [MaxLength(16)]
        public string Code { get; set; }
        public int? ParentId { get; set; }
        public int? MainFeatureId { get; set; }
        public virtual Category Parent { get; set; }
        public DateTime CreateDate { get; set; }
        public string Remark { get; set; }
        //public virtual ICollection<CategoryFeature> Features { get; set; }
       // public virtual ICollection<CategoryMainFeature> CategoryMainFeatures { get; set; }

     //   public virtual ICollection<CategoryAttachment> CategoryAttachments { get; set; }
   //     public virtual ICollection<MainFeatureAttachment> MainFeatureAttachments { get; set; }

 //       public virtual ICollection<Product> Products { get; set; }
        public int? SortOrder { get; set; }
        public Category()
        {
            //Features = new HashSet<CategoryFeature>();
            //CategoryAttachments = new HashSet<CategoryAttachment>();
            //Products = new HashSet<Product>();
            //CategoryMainFeatures = new HashSet<CategoryMainFeature>();
            //MainFeatureAttachments = new HashSet<MainFeatureAttachment>();

        }
    }
}
