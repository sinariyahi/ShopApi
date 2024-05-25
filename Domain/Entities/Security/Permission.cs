using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security
{
    [Table("Permissions", Schema = "Security")]
    public class Permission
    {
        public int Id { get; set; }
        [Required, MaxLength(64)]
        public string Title { get; set; }
        [MaxLength(64)]
        public string EnTitle { get; set; }
        [MaxLength(128)]
        public string Url { get; set; }
        public bool ShowInSidebar { get; set; }
        public int? SortOrder { get; set; }
        [MaxLength(256)]
        public string Icon { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsTopLevel { get; set; }
        public int? PermissionGroupId { get; set; }
        public PermissionGroup PermissionGroup { get; set; }
       public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
