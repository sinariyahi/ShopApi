using Domain.Entities.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Common;
using Domain.Entities.Support;

namespace Domain.Entities.Security
{
    [Table("Users", Schema = "Security")]
    public class User
    {
        public Guid Id { get; set; }
        [Required, MaxLength(128)]
        public string UserName { get; set; }
        [Required, MaxLength(128)]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public bool? IsCompany { get; set; }
        public string LastName { get; set; }
        [MaxLength(256)]
        public string FullName { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(64)]
        public string Email { get; set; }

        [MaxLength(32)]
        public string Code { get; set; }

        public bool IsConfirmPhoneNumber { get; set; } = false;
        [MaxLength(32)]
        public string PhoneNumber { get; set; }
        public Sex? Sex { get; set; }
        public UserType? UserType { get; set; }
        public Guid? ConfirmationCode { get; set; }
        public DateTime? ConfirmationDate { get; set; }


        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastTryLoginDate { get; set; }

        public int LoginfailedCount { get; set; } = 0;

        public string EmailSignature { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<UserProject> UserProjects { get; set; }
        public ICollection<UserDiscipline> UserDisciplines { get; set; }
        public ICollection<UserOrganizationUnit> UserOrganizationUnits { get; set; }
        public ICollection<UserAnswer> UserAnswers { get; set; }
        public ICollection<UserCartable> UserCartables { get; set; }
        public ICollection<UserPayment> UserPayments { get; set; }


        public User()
        {
            UserProjects = new HashSet<UserProject>();
            UserDisciplines = new HashSet<UserDiscipline>();
            UserOrganizationUnits = new HashSet<UserOrganizationUnit>();
            UserAnswers = new HashSet<UserAnswer>();
            UserCartables = new HashSet<UserCartable>();
            UserPayments = new HashSet<UserPayment>();

        }
    }
}
