using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public static class AmountUtility
    {
        public static string ConvertToBalanceAmount(double amount, AmountUnit amountUnit)
        {
            return ConvertToBalanceAmount(amount.ToString(), amountUnit);
        }

        public static string ConvertToBalanceAmount(string amount, AmountUnit amountUnit)
        {
            if (string.IsNullOrEmpty(amount)) return "";

            var doubleAmount = Convert.ToDouble(amount);

            doubleAmount = ConvertToAmountUnit(doubleAmount, amountUnit).Value;

            bool isNegative = false;

            if (doubleAmount < 0) isNegative = true;

            return !isNegative ? string.Format("{0:n0}", doubleAmount) : string.Format("({0:n0})", doubleAmount).Replace("-", "");
        }

        public static double? ConvertToAmountUnit(double? amount, AmountUnit amountUnit)
        {
            if (amount == null) return (double?)null;
            double finalAmount = 0;
            switch (amountUnit)
            {
                case AmountUnit.Default:
                    finalAmount = amount.Value;
                    break;
                case AmountUnit.MilionRial:
                    finalAmount = amount.Value / 1000000;
                    break;
                case AmountUnit.MilyardRial:
                    finalAmount = amount.Value / 1000000000;
                    break;
            }
            return Math.Truncate(100 * finalAmount) / 100;
        }
    }
}
