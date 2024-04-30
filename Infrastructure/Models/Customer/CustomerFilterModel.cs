using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Customer
{
    public class CustomerFilterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class CustomerDto
    {
        public int? GoldIranCityId { get; set; }
        public string PostCode { get; set; }

        public int? GoldIranProvinceId { get; set; }
        public int? RegionId { get; set; }
        public int? ParishId { get; set; }

        public string DeliveryAddress { get; set; }
        public Guid Id { get; set; }
        [Required, MaxLength(32)]
        public string UserName { get; set; }

        [Required, MaxLength(64)]
        public string FirstName { get; set; }
        [Required, MaxLength(64)]
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime CreateDate { get; set; }
        public string RegisterDate { get; set; }

        public bool IsActive { get; set; }
        public string IsActiveTitle { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Sex? Sex { get; set; }
        public string SexTitle { get; set; }
        public string Code { get; set; }
        public UserType? UserType { get; set; }
        public string UserTypeTitle { get; set; }
        public string ShamsiCreateDate { get; set; }
        public string EmailSignature { get; set; }
        public string Address { get; set; }
        //public Province? Province { get; set; }
        //public int? CityId { get; set; }

    }

    public class CustomerInputModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }

        [MaxLength(32), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, MaxLength(64)]
        public string FirstName { get; set; }
        [Required, MaxLength(64)]
        public string LastName { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Sex? Sex { get; set; }
        public Province Province { get; set; }
        public int CityId { get; set; }
        public string CityTitle { get; set; }
        public string ProvinceTitle { get; set; }

        public int? GoldIranCityId { get; set; }

        public int? GoldIranProvinceId { get; set; }
        public int? RegionId { get; set; }
        public int? ParishId { get; set; }
        public string RegionTitle { get; set; }
        public string ParishTitle { get; set; }
        public string DeliveryAddress { get; set; }
        public string PostCode { get; set; }

    }

    public class RegisterCustomerInputModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Captcha { get; set; }
        public string CaptchaKey { get; set; }

    }

}
