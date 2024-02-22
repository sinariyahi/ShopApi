using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security
{
    [Table("UserOrganizationUnits", Schema = "Security")]
    public class UserOrganizationUnit
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public int OrganizationUnitId { get; set; }
        public virtual Role OrganizationUnit { get; set; }
    }
}
