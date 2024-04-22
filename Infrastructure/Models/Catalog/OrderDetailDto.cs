using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Catalog
{
    public class OrderDetailDto
    {
        public string EnTitle { get; set; }
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Guid OrderId { get; set; }
        public long ItemCount { get; set; }
        public long Price { get; set; }
        public long DiscountedPrice { get; set; }
        public string BrandName { get; set; }
        public string ProductName { get; set; }
        public string CoverFile { get; set; }
    }

    public class UserOrderDetailDto
    {
        public int ProductId { get; set; }
        public long Price { get; set; }
        public long ItemCount { get; set; }
    }


}
