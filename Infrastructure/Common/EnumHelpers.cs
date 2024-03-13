using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public static class EnumHelpers
    {
        public static string GetNameAttribute<T>(this T enumValue) where T : Enum
        {
            if (enumValue == null) return String.Empty;
            var item = typeof(T).GetMember(enumValue.ToString()).FirstOrDefault();
            if (item == null)
                return string.Empty;
            return item.GetCustomAttribute<DisplayAttribute>().Name;

        }
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

    }
}
