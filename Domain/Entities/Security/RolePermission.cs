using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security
{
    [Table("RolePermissions", Schema = "Security")]
    public class RolePermission
    {
        public int Id { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
