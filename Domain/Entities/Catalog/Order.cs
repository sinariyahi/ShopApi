using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Common;
using Domain.Entities.Security;

namespace Domain.Entities.Catalog
{
    [Table("Orders", Schema = "Catalog")]
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int? KoponId { get; set; }
        public virtual Kopon Kopon { get; set; }

        public string OrderNumber { get; set; }
        public DateTime CreateDate { get; set; }

        public DateTime? InProgressDate { get; set; }
        public DateTime? RejectDate { get; set; }
        public DateTime? SendingDate { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? DeliveredDate { get; set; }

        public int KoponPercent { get; set; }
        public long KoponAmount { get; set; }
        public long FinalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string OrderStatusRemark { get; set; }

        public int? CityId { get; set; }

        public int? ProvinceId { get; set; }
        public int? RegionId { get; set; }
        public int? ParishId { get; set; }


        public string CityTitle { get; set; }

        public string ProvinceTitle { get; set; }
        public string RegionTitle { get; set; }
        public string ParishTitle { get; set; }

        public string DeliveryAddress { get; set; }

        public string OrderReturnByCustomerRemark { get; set; }
        public DateTime? OrderReturnByCustomerDate { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<OrderLog> OrderLogs { get; set; }

        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            OrderLogs = new HashSet<OrderLog>();

        }

    }
}
