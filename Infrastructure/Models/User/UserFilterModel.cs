using Infrastructure.Common;
using Infrastructure.Models.EIED;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.User
{
    public class UserFilterModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserType? UserType { get; set; }
    }
    public class UserDto : ComboItemDto
    {
        public Guid Id { get; set; }
        [Required, MaxLength(32)]
        public string UserName { get; set; }
        [MaxLength(32), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, MaxLength(64)]
        public string FirstName { get; set; }
        [Required, MaxLength(64)]
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveTitle { get; set; }
        public int[] Cartables { get; set; }
        public string[] CartableNames { get; set; }
        public int[] Projects { get; set; }
        public string[] ProjectNames { get; set; }
        public int[] Disciplines { get; set; }
        public string[] DisciplineNames { get; set; }
        public int[] OrganizationUnits { get; set; }
        public string[] OrganizationUnitNames { get; set; }
        public int[] Roles { get; set; }
        public string[] RolesName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Sex? Sex { get; set; }
        public string SexTitle { get; set; }
        public string Code { get; set; }
        public UserType? UserType { get; set; }
        public string UserTypeTitle { get; set; }
        public string ShamsiCreateDate { get; set; }
        public string EmailSignature { get; set; }
    }

    public class UserInputModel
    {
        public Guid? Id { get; set; }
        public string UserName { get; set; }
        [MaxLength(32), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, MaxLength(64)]
        public string FirstName { get; set; }
        [Required, MaxLength(64)]
        public string LastName { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public int[]? Projects { get; set; }
        public int[]? Cartables { get; set; }
        public int[]? Disciplines { get; set; }
        public int[]? OrganizationUnits { get; set; }
        public int[]? Roles { get; set; }
        public string PhoneNumber { get; set; }
        public Sex Sex { get; set; }
        public string SexTitle { get; set; }
        public string Code { get; set; }
        public UserType UserType { get; set; }
    }
}
