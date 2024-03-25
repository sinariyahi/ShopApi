using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public static class RegexUtility
    {
        public static bool IsValidPhoneNumber(this string input)
        {
            string pattern = @"^09[0|1|2|3][0-9]{8}$";
            Regex reg = new Regex(pattern);
            return reg.IsMatch(input);
        }
        public static bool IsValidEmail(this string input)
        {
            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            Regex reg = new Regex(pattern);
            return reg.IsMatch(input);
        }

    }
}
