using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Catalog
{
    public class FeatureProductDto
    {
        public string Key { get; set; }

        public string FeatureValue { get; set; }
        public string BrandName { get; set; }
        public string ProductName { get; set; }
        public string ProductEnName { get; set; }

        public List<FeatureProductDto> Brands { get; set; } = new List<FeatureProductDto>();
    }
}
