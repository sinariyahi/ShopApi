using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public static class ImageUtility
    {
        public static string ConvertToBase64String(byte[] image)
        {
            return "data:image/png;base64," + Convert.ToBase64String(image);
        }
    }
}
