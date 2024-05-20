using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.User
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public string ParentTitle { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public Guid UserId { get; set; }
        public OrganizationLevel? OrganizationLevel { get; set; }
    }

    public class TreeDto
    {
        public int Key { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string EnTitle { get; set; }
        public string FeatureTitle { get; set; }
        public string File { get; set; }
        //public List<int> Ids { get; set; } = new List<int>();
        public string SymbolTitle { get; set; }
        public int Rate { get; set; }
        public List<TreeDto> Children { get; set; } = new List<TreeDto>();
        public int? ParentId { get; set; }
        public string BrandTitle { get; set; }
        public string Lable { get; set; }

        public string Text { get; set; }
        public int Value { get; set; }
        public int FeatureCount { get; set; }
    }

    public class TreeItemDto
    {
        public string EnTitle { get; set; }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public int? ParentId { get; set; }
        public int? SortOrder { get; set; }
    }

    public class RoleGroupPermissionDto
    {
        public int Value { get; set; }
        public string Label { get; set; }
        public string EnTitle { get; set; }
        public bool Checked { get; set; }
        public List<RolePermissionDto> Children { get; set; } = new List<RolePermissionDto>();
    }

    public class RolePermissionDto
    {
        public int Value { get; set; }
        public string Label { get; set; }
        public string EnTitle { get; set; }
        public bool Checked { get; set; }
    }
}
