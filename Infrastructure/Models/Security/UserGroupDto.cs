using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Security
{
    public class UserGroupDto
    {
        public Guid Id { get; set; }

        [Required, MaxLength(64)]
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveTitle { get; set; }
        public List<Guid> Users { get; set; }
        public List<CompanyRequestWorkFlowStatus> Actions { get; set; }

    }

    public class UserAccessForUserGroupDto
    {

        public Guid UserId { get; set; }

        public Guid UserGroupId { get; set; }
        public List<CompanyRequestWorkFlowStatus> Actions { get; set; }

        public List<int> DisciplineIds { get; set; }
        public List<string> DisciplineNames { get; set; }


    }

    public class UserGroupActionsDto
    {

        public Guid UserId { get; set; }

        public Guid UserGroupId { get; set; }

        public CompanyRequestWorkFlowStatus Status { get; set; }

    }

}
