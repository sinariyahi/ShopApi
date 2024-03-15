using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public static class ExcelUtility
    {
        public static byte[] ExportToExcel<T>(List<T> table, string filename)
        {
            using ExcelPackage pack = new ExcelPackage();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(filename);
            try
            {
                ws.Cells["A1"].LoadFromCollection(table, true, TableStyles.Light1);

            }
            catch (Exception ex)
            {

                throw;
            }
            return pack.GetAsByteArray();
        }

        public static string GenerateExcelFileName(string name)
        {
            var random = new Random();
            return $"{name}_{DateUtility.GetShamsiCurrentDate()}_{random.Next(9999)}.xlsx";
        }
    }
}
