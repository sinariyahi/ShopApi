using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security
{
    [Table("Roles", Schema = "Security")]
    public class Role
    {
        public int Id { get; set; }

        [Required, MaxLength(64)]
        public string RoleName { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }

        [MaxLength(32)]
        public string Code { get; set; }

        [MaxLength(64)]
        public string HomaCode { get; set; }

        public int? ParentId { get; set; }
        public virtual Role Parent { get; set; }

     //   public OrganizationLevel? OrganizationLevel { get; set; }

        public bool IsActive { get; set; }

       // public ICollection<UserRole> UserRoles { get; set; }
      //  public ICollection<RolePermission> RolePermissions { get; set; }
        public ICollection<MenuRole> MenuRoles { get; set; }

    }
}
