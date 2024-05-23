using Domain.Entities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Catalog
{
    [Table("ProductFeatureValues", Schema = "Catalog")]
    public class ProductFeatureValue
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int ProductCategoryFeatureId { get; set; }
        public CategoryFeature ProductCategoryFeature { get; set; }
        public string FeatureValue { get; set; }
        public int? FeatureValueNumber { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
