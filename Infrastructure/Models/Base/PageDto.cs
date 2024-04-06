using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Base
{
    public class PageDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Link { get; set; }

        public bool IsActive { get; set; }
        public string IsActiveTitle { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public PagesLinkType PagesLinkType { get; set; }
        public string PagesLinkTypeTitle { get; set; }


    }

    public class UserPageDto
    {
        public Guid Id { get; set; }
        public string Link { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string File { get; set; }
        public PagesLinkType PagesLinkType { get; set; }

    }

    public class PageInputModel
    {
        public Guid Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }

        public bool IsActive { get; set; }
        public string IsActiveTitle { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }

        public PagesLinkType PagesLinkType { get; set; }
    }
}
