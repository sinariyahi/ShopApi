using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Project
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ProjectTitle { get; set; }
        public Boolean IsActive { get; set; } = true;
        public string Description { get; set; }
        public string Code { get; set; }
        public Guid UserId { get; set; }
        public int OrganizationUnitId { get; set; }
        public string OrganizationUnitTitle { get; set; }
    }
}
