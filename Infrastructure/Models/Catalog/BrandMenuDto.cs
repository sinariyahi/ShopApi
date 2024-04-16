using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Catalog
{
    public class BrandMenuDto
    {
        public int Key { get; set; }
        public string Title { get; set; }
        public string EnTitle { get; set; }

        public List<BrandMenuDto> Children { get; set; } = new List<BrandMenuDto>();
    }

    public class BrandForDashboardChart
    {
        public int Id { get; set; }
        public string X { get; set; }
        public int Y { get; set; }
    }

    public class BrandDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string EnTitle { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveTitle { get; set; }
        public string Description { get; set; }
        public List<FileItemDto> File { get; set; }
        public int? SortOrder { get; set; }

    }

    public class UserBrandDto
    {
        public int Id { get; set; }
        public string EnTitle { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string File { get; set; }
        public int? SortOrder { get; set; }

    }

    public class BrandInputModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string EnTitle { get; set; }

        public bool IsActive { get; set; }
        public string IsActiveTitle { get; set; }
        public string Description { get; set; }
        public List<IFormFile> File { get; set; }
        public int? SortOrder { get; set; }

    }
}
