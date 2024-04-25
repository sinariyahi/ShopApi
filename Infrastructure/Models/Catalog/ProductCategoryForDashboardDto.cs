using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Catalog
{
    public class ProductCategoryForDashboardDto
    {
        public int ProductCategoryCount { get; set; }
        public List<ProductCategoryForDashboardItemDto> Items { get; set; } = new List<ProductCategoryForDashboardItemDto>();
    }

    public class ProductCategoryForDashboardItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Value { get; set; }
    }
}
