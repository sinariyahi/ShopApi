using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Base
{
    public class SocialMediaDto
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Link { get; set; }

        public bool IsActive { get; set; }
        public string IsActiveTitle { get; set; }
        public string Description { get; set; }

        public List<FileItemDto> File { get; set; }
    }

    public class UserSocialMediaDto
    {
        public int Id { get; set; }
        public string Link { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string File { get; set; }

    }

    public class SocialMediaInputModel
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }

        public List<IFormFile> File { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveTitle { get; set; }
        public string Description { get; set; }

    }
}
