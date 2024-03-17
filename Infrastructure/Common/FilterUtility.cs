using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public static class FilterUtility
    {
        public static T ConvertToModel<T>(GridQueryModel model)
        {
            var result = (T)Activator.CreateInstance(typeof(T));
            foreach (var item in result.GetType().GetProperties())
            {
                var filterItem = model.Filtered.FirstOrDefault(q => q.column.ToUpper() == item.Name.ToUpper());
                if (filterItem != null)
                {
                    item.SetValue(result, filterItem.value);
                }
            }

            return result;
        }
    }
}
