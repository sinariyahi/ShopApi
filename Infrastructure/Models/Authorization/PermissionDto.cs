using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Authorization
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public ActionType ActionType { get; set; }
        public string Url { get; set; }
        public bool ShowInSidebar { get; set; }
        public bool ShowInDashboard { get; set; }
        public int? SortOrder { get; set; }
        public string GroupName { get; set; }
        public bool Checked { get; set; }
    }

    public class PermissionGroupDto
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public int? SortOrder { get; set; }
        public IEnumerable<PermissionDto> Permissions { get; set; }
    }

    public class RolePermissionInputModel
    {
        public int RoleId { get; set; }
        public List<int> Permissions { get; set; }
    }
}
