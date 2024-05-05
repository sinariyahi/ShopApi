using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.OrganizationUnit
{
    public class OrganizationUnitDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Boolean IsActive { get; set; } = true;
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public int[] Disciplines { get; set; }
        public string[] DisciplineTitles { get; set; }
        public string Code { get; set; }
    }

    public class DisciplineDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
    }

    public class AreaDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
    }
}
