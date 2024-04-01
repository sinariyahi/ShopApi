using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Base
{
    public class CityDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsActive { get; set; }
        public string IsActiveTitle { get; set; }
        public string ProvinceTitle { get; set; }
        public Province ProvinceId { get; set; }

    }


    public class CityInputModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Province ProvinceId { get; set; }
        public string ProvinceTitle { get; set; }

        public bool IsActive { get; set; }
        public string IsActiveTitle { get; set; }

    }
}
