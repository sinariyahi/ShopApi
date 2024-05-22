using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Catalog
{
    [Table("CategoryFeatures", Schema = "Catalog")]
    public class CategoryFeature
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int FeatureId { get; set; }
        public virtual Feature Feature { get; set; }
        public int? SortOrder { get; set; }
    }

    [Table("CategoryMainFeatures", Schema = "Catalog")]
    public class CategoryMainFeature
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int FeatureId { get; set; }
        public virtual Feature Feature { get; set; }
        public int? SortOrder { get; set; }
    }
}
