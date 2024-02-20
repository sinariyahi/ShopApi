using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security
{
    [Table("UserGroups", Schema = "Security")]
    public class UserGroup
    {
        public Guid Id { get; set; }

        [Required, MaxLength(64)]
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
    //    public virtual ICollection<UserGroupMember> UserGroupMembers { get; set; }
        public virtual ICollection<UserGroupAction> UserGroupActions { get; set; }

        public UserGroup()
        {
  //          UserGroupMembers = new HashSet<UserGroupMember>();
            UserGroupActions = new HashSet<UserGroupAction>();
        }

    }

    [Table("UserGroupActions", Schema = "Security")]
    public class UserGroupAction
    {
        public int Id { get; set; }
//        public CompanyRequestWorkFlowStatus CompanyRequestWorkFlowStatus { get; set; }
        public Guid UserGroupId { get; set; }
        public virtual UserGroup UserGroup { get; set; }
        //public virtual ICollection<UserGroupMember> UserGroupMembers { get; set; }

        //public UserGroupAction()
        //{
        //    UserGroupMembers = new HashSet<UserGroupMember>();

        //}
    }
}
