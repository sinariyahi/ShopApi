using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Catalog
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Province? Province { get; set; }
        public string ProvinceTitle { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public int? KoponId { get; set; }
        public string CreateDate { get; set; }
        public string KoponTitle { get; set; }
        public string OrderNumber { get; set; }
        public int KoponPercent { get; set; }
        public long KoponAmount { get; set; }
        public long FinalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string OrderStatusTitle { get; set; }
        public string OrderStatusRemark { get; set; }
        public string OrderReturnRemark { get; set; }
        public string OrderReturnDate { get; set; }

        public List<OrderDetailDto> Items { get; set; }

        public string PostCode { get; set; }

        public int? CityId { get; set; }

        public int? ProvinceId { get; set; }
        public int? RegionId { get; set; }
        public int? ParishId { get; set; }

        public string CityTitle { get; set; }

        public string RegionTitle { get; set; }
        public string ParishTitle { get; set; }
        public string DeliveryAddress { get; set; }
    }
    public class OrderFilterDto : GridQueryModel
    {
        public OrderStatus? OrderStatus { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerName { get; set; }
        public Province? Province { get; set; }
        public string OrderNumber { get; set; }


    }



    public class OrderModel
    {
        public Guid Id { get; set; }
        public int? KoponId { get; set; }
        public string CreateDate { get; set; }
        public string KoponTitle { get; set; }
        public string OrderNumber { get; set; }
        public int KoponPercent { get; set; }
        public long KoponAmount { get; set; }
        public long FinalAmount { get; set; }
        public string OrderStatusTitle { get; set; }
        public string OrderStatusRemark { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<OrderDetailDto> Items { get; set; }

    }



    public class UserOrderDto
    {
        public string KoponCode { get; set; }
        //public  string CityTitle { get; set; }
        //public string ProvinceTitle { get; set; }

        public bool IsPaied { get; set; } = false;
        public int? CityId { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public int? ProvinceId { get; set; }
        public int? RegionId { get; set; }
        public int? ParishId { get; set; }

        //public string CityTitle { get; set; }

        //public string RegionTitle { get; set; }
        //public string ParishTitle { get; set; }

        //public string ProvinceTitle { get; set; }


        public string DeliveryAddress { get; set; }
        public List<UserOrderDetailDto> Items { get; set; }
    }


    public class OrderInputDto
    {
        public string Id { get; set; }
        public string OrderStatusRemark { get; set; }
        public OrderStatus OrderStatus { get; set; }

    }

    public class ReturnOrderInputDto
    {
        public string Id { get; set; }
        public string Remark { get; set; }

    }

}
