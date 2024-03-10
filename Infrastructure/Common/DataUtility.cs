using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public static class DataUtility
    {
        public static string CheckNullExcelData(this object data)
        {
            return (data == null && data.ToString().Length == 0 && data.ToString() != @"""" && data.ToString() != @"''") ? "-" : data.ToString().Trim();
        }

        public static string RemoveDashForTitle(string Title)
        {
            var str = Title.Replace('-', ' ');

            return str;
        }


        public static int? GetFeatureNumberValue(string data)
        {
            int result;
            if (int.TryParse(data, out result))
            {
                return result;
            }
            return null;
        }
    }
}
