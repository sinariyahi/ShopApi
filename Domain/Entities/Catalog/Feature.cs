using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Catalog
{
    [Table("Features", Schema = "Catalog")]
    public class Feature
    {
        public int Id { get; set; }
        [Required, MaxLength(128)]
        public string Title { get; set; }
        [MaxLength(256)]
        public string Remark { get; set; }
     //   public UnitType UnitType { get; set; }
      //  public ControlType ControlType { get; set; }
        public DateTime CreateDate { get; set; }
        public int? FeatureCategoryId { get; set; }
     //   public virtual FeatureCategory FeatureCategory { get; set; }
        public int? Max { get; set; }
        public int? Min { get; set; }
        public string Option { get; set; }
        public bool ShowInFilter { get; set; } = false;

        public int? SymbolId { get; set; }
     //   public virtual Symbol Symbol { get; set; }
    }
}
