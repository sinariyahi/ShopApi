using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security
{
    [Table("UserGroupMembers", Schema = "Security")]
    public class UserGroupMember
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }
        public virtual ICollection<UserGroupAction> UserGroupActions { get; set; }

        public UserGroupMember()
        {
            UserGroupActions = new HashSet<UserGroupAction>();
        }

    }
}
