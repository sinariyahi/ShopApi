using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Component
{
    public class ComponentFilterDto
    {
        public int Id { get; set; }
        public FilterType FilterType { get; set; }
        public string Lable { get; set; }
        //[JsonIgnore]
        public DataSourceType DataSourceType { get; set; }
        public string DataSourceCommand { get; set; }
        public IEnumerable<KeyValueDto> Data { get; set; }
    }

    public class ComponentDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Type { get; set; }

        public int DataSourceType { get; set; }

        public string DataSourceCommand { get; set; }

        public bool IsActive { get; set; }
        public Guid Code { get; set; }

        public IEnumerable<ComponentFilterDto> Filters { get; set; }
        public IEnumerable<ComponentColumnsDto> Columns { get; set; }

    }

    public class ComponentColumnsDto
    {

        public int Id { get; set; }

        public string Text { get; set; }


        public string DataField { get; set; }

        public bool IsPk { get; set; }

        public bool IsFilter { get; set; }

        public string Format { get; set; }
        public bool IsActive { get; set; }
        public int ComponentId { get; set; }

    }

}
