using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGenericQueryService<T> where T : class
    {
        Task<ShopActionResult<List<T>>> QueryAsync(GridQueryModel args, IList<string> fields = null, IList<string> includes = null, bool exportToExcel = false);
    }
}
