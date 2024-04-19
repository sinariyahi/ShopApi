using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Catalog
{
    public class FeatureDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Remark { get; set; }
        public UnitType UnitType { get; set; }
        public ControlType ControlType { get; set; }
        public string UnitTypeTitle { get { return UnitType.GetNameAttribute(); } }
        public string ControlTypeTitle { get { return ControlType.GetNameAttribute(); } }
        public int? Max { get; set; }
        public int? Min { get; set; }
        public int? FeatureCategoryId { get; set; }
        public int? FeatureId { get; set; }
        public bool ShowInFilter { get; set; }


        public int? SymbolId { get; set; }
        public string SymbolTitle { get; set; }
        public string Option { get; set; }

    }

    public class UserFeatureDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public string EnTitle { get; set; }
        public int FeatureId { get; set; }

        public string FeatureTitle { get; set; }

        public string MainFeatureFile { get; set; }
        public int? MainFeatureId { get; set; }
        public int CategoryId { get; set; }

        public string FeatureValue { get; set; }
     //   public List<TreeDto> Children { get; set; }
        public string Option { get; set; }

    }
}
