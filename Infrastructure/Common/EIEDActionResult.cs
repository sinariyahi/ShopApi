using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class ShopActionResult<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public int Total { get; set; } = 0;
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public List<DataGridColumn> Columns { get; set; } = new List<DataGridColumn>();
    }

    public class DataGridColumn
    {
        public string Header { get; set; }
        public string Field { get; set; }
    }

    public class GridQueryModel
    {
        public int Size { get; set; } = 10;
        public int Page { get; set; } = 1;
        public SortModel[] Sorted { get; set; } = new SortModel[]
        {
            new SortModel
            {
                column = "Id",
                desc = true
            }
        };
        public FilterModel[] Filtered { get; set; } = new FilterModel[] { };
    }

    public class FilterModel
    {
        public string column { get; set; }
        public string value { get; set; }
    }

    public class SortModel
    {
        public string column { get; set; }
        public bool desc { get; set; } = false;
    }


}
