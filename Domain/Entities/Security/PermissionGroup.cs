using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security
{
    [Table("PermissionGroups", Schema = "Security")]
    public class PermissionGroup
    {
        public int Id { get; set; }
        [Required, MaxLength(64)]
        public string Title { get; set; }
        [MaxLength(64)]
        public string EnTitle { get; set; }
        [Required, MaxLength(256)]
        public string Icon { get; set; }
        public bool ShowInMenu { get; set; }
        public int? SortOrder { get; set; }
        public bool? IsActive { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
