using Infrastructure.Models.Common;
using Infrastructure.Models.EIED;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public static class DateUtility
    {
        public static IEnumerable<(string Month, int Year)> MonthsBetween(DateTime startDate, DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }

            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            while (iterator <= limit)
            {
                yield return (
                    dateTimeFormat.GetMonthName(iterator.Month),
                    iterator.Year
                );

                iterator = iterator.AddMonths(1);
            }
        }

        public static string CovertToShamsi(DateTime? datetime)
        {
            if (datetime == null) return "";
            var pc = new PersianCalendar();
            var convertedDateTime = Convert.ToDateTime(datetime);
            var year = pc.GetYear(convertedDateTime).ToString();
            var month = pc.GetMonth(convertedDateTime).ToString().PadLeft(2, '0');
            var day = pc.GetDayOfMonth(convertedDateTime).ToString().PadLeft(2, '0');

            var min = convertedDateTime.Minute.ToString();
            var hour = convertedDateTime.Hour.ToString();

            if (hour != "0")
            {
                return $"{year}-{month}-{day}-{hour}:{min}";

            }
            else
            {
                return $"{year}-{month}-{day}";

            }

        }


        public static string GetMiladiMonthName(int month, int year)
        {
            DateTime date = new DateTime(year, month, 1);

            return date.ToString("MMMM");
        }

        public static double ConvertToFloorDate(DateTime date)
        {
            return Math.Floor(date.ToOADate());
        }

        public static string RemoveDash(this DateTime dateTime)
        {
            var year = dateTime.Year;
            var month = dateTime.Month.ToString().Length == 1 ? $"0{dateTime.Month}" : dateTime.Month.ToString();
            var day = dateTime.Day.ToString().Length == 1 ? $"0{dateTime.Day}" : dateTime.Day.ToString();
            return $"{year}{month}{day}";
        }


        public static string Remove_Dash(DateTime dateTime)
        {
            var year = dateTime.Year;
            var month = dateTime.Month.ToString().Length == 1 ? $"0{dateTime.Month}" : dateTime.Month.ToString();
            var day = dateTime.Day.ToString().Length == 1 ? $"0{dateTime.Day}" : dateTime.Day.ToString();
            return $"{year}{month}{day}";
        }




        public static string ToShamsi(this DateTime datetime, bool reverse = false)
        {
            var pc = new PersianCalendar();
            var convertedDateTime = Convert.ToDateTime(datetime);
            var year = pc.GetYear(convertedDateTime).ToString();
            var month = pc.GetMonth(convertedDateTime).ToString().PadLeft(2, '0');
            var day = pc.GetDayOfMonth(convertedDateTime).ToString().PadLeft(2, '0');
            if (reverse)
                return $"{day}-{month}-{year}";
            else
                return $"{year}-{month}-{day}";

        }

        public static string GetShamsiCurrentDate()
        {
            var pc = new PersianCalendar();
            var convertedDateTime = Convert.ToDateTime(DateTime.Now);
            var year = pc.GetYear(convertedDateTime).ToString();
            var month = pc.GetMonth(convertedDateTime).ToString().PadLeft(2, '0');
            var day = pc.GetDayOfMonth(convertedDateTime).ToString().PadLeft(2, '0');
            return $"{year}_{month}_{day}";
        }




        public static string MiladiToShamsi(this DateTime datetime, bool reverse = false)
        {
            var pc = new PersianCalendar();
            var convertedDateTime = Convert.ToDateTime(datetime);
            var year = pc.GetYear(convertedDateTime).ToString();
            var month = pc.GetMonth(convertedDateTime).ToString().PadLeft(2, '0');
            var day = pc.GetDayOfMonth(convertedDateTime).ToString().PadLeft(2, '0');
            if (reverse)
                return $"{day}/{month}/{year}";
            else
                return $"{year}/{month}/{day}";

        }

        public static string ToShamsi(this DateTime? datetime, bool reverse = false)
        {
            if (datetime == null) return "";
            var pc = new PersianCalendar();
            var convertedDateTime = Convert.ToDateTime(datetime);
            var year = pc.GetYear(convertedDateTime).ToString();
            var month = pc.GetMonth(convertedDateTime).ToString().PadLeft(2, '0');
            var day = pc.GetDayOfMonth(convertedDateTime).ToString().PadLeft(2, '0');
            if (reverse)
                return $"{day}/{month}/{year}";
            else
                return $"{year}/{month}/{day}";

        }

        public static string FormatDate(DateTime? datetime)
        {
            if (datetime != null)
            {
                var year = datetime.Value.Year.ToString();
                var month = datetime.Value.Month.ToString();
                var day = datetime.Value.Day.ToString();
                return $"{year}-{month}-{day}";
            }

            return string.Empty;

        }
        public static string FormatDateTime(DateTime? datetime)
        {
            if (datetime != null)
            {
                var year = datetime.Value.Year.ToString();
                var month = datetime.Value.Month.ToString();
                var day = datetime.Value.Day.ToString();

                var min = datetime.Value.Minute.ToString();
                var hour = datetime.Value.Hour.ToString();

                return $"{year}-{month}-{day}-{hour}:{min}";
            }

            return string.Empty;

        }


        public static string FormatShamsiDateTime(DateTime datetime)
        {

            var pc = new PersianCalendar();

            var year = pc.GetYear(datetime).ToString();
            var month = pc.GetMonth(datetime).ToString().PadLeft(2, '0');
            var day = pc.GetDayOfMonth(datetime).ToString().PadLeft(2, '0');

            var min = datetime.Minute.ToString();
            var hour = datetime.Hour.ToString();

            return $"{year}-{month}-{day}-{hour}:{min}";

        }


        public static string GetDateWithShowTrend(ShowTrends showTrend, int? dateValue, int? month, int shamsiYear)
        {
            string result = string.Empty;
            switch (showTrend)
            {
                case ShowTrends.Daily:
                    result = $"{dateValue} {month.Value.ToShamsiMonthName()}-{shamsiYear}";
                    break;
                case ShowTrends.Monthly:
                    result = $"{month.Value.ToShamsiMonthName()}-{shamsiYear}";
                    break;
                case ShowTrends.ThreeMonths:
                    result = $"{EnumHelpers.GetNameAttribute((Season)dateValue.Value)}-{shamsiYear}";
                    break;
                case ShowTrends.SixMonths:
                    {
                        string temp = dateValue.Value == 1 ? "شش ماهه اول" : "شش ماهه دوم";
                        result = $"شش ماهه {temp}-{shamsiYear}";
                        break;
                    }
                case ShowTrends.Yearly:
                    result = shamsiYear.ToString();
                    break;
            }

            return result;
        }

        public static string ToTwoCharacterMonth(this string month)
        {
            return month.Length == 1 ? $"0{month}" : month;
        }

        public static int ToShamsiYear(this DateTime datetime)
        {
            var pc = new PersianCalendar();
            var convertedDateTime = Convert.ToDateTime(datetime);
            return pc.GetYear(convertedDateTime);

        }

        public static string ToShamsiStartCurrentYear(this DateTime datetime)
        {
            var year = ToShamsiYear(datetime);
            return $"{year}/01/01";
        }

        public static DateTime ToMiladiFromStartShamsiYear(this DateTime datetime)
        {
            var shamsiDate = datetime.ToShamsiStartCurrentYear();
            return shamsiDate.ToDateTime();
        }

        public static int ToShamsiMonth(this DateTime datetime)
        {
            var pc = new PersianCalendar();
            var convertedDateTime = Convert.ToDateTime(datetime);
            return pc.GetMonth(convertedDateTime);

        }

        public static Season ToShamsiSeason(this DateTime datetime)
        {
            var currentShamsiMonth = datetime.ToShamsiMonth();
            Season currentSeason = Season.Spring;
            switch (currentShamsiMonth)
            {
                case 1:
                case 2:
                case 3:
                    {
                        currentSeason = Season.Spring;
                        break;
                    }
                case 4:
                case 5:
                case 6:
                    {
                        currentSeason = Season.Summer;
                        break;
                    }
                case 7:
                case 8:
                case 9:
                    {
                        currentSeason = Season.Autumn;
                        break;
                    }
                case 10:
                case 11:
                case 12:
                    {
                        currentSeason = Season.Winter;
                        break;
                    }
            }
            return currentSeason;
        }

        public static int ToShamsiSixMonthSeason(this DateTime datetime)
        {
            int value = 1;
            var currentShamsiMonth = datetime.ToShamsiMonth();
            switch (currentShamsiMonth)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    {
                        value = 1;
                        break;
                    }
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    {
                        value = 2;
                        break;
                    }
            }
            return value;
        }

        public static List<string> Months = new List<string> { "", "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };

        public static string GetMonthName(int month)
        {
            return Months[month];
        }

        public static List<EIEDEnums> GetShamsiMonths()
        {
            var months = new List<EIEDEnums>();
            for (int i = 1; i <= 12; i++)
            {
                months.Add(new EIEDEnums
                {
                    Value = i,
                    Text = Months[i],
                });
            }
            return months;
        }

        public static int ToShamsiCurrentYear(this DateTime datetime)
        {
            var pc = new PersianCalendar();
            var convertedDateTime = Convert.ToDateTime(datetime);
            int year = pc.GetYear(convertedDateTime);
            return year;
        }

        public static string ToShamsiMonthName(this DateTime datetime)
        {
            var pc = new PersianCalendar();
            var convertedDateTime = Convert.ToDateTime(datetime);
            int month = pc.GetMonth(convertedDateTime);
            return Months[month];
        }

        public static string ToShamsiMonthName(this int monthNumber)
        {
            return Months[monthNumber];
        }

        public static int ToShamsiDay(this DateTime datetime)
        {
            var pc = new PersianCalendar();
            var convertedDateTime = Convert.ToDateTime(datetime);
            return pc.GetDayOfMonth(convertedDateTime);

        }

        //public static string ToShamsiDayName(this DateTime datetime)
        //{
        //    return datetime.ToFa("D");
        //}

        public static DateTime ToDateTime(this string shamsiDatetime)
        {
            var pc = new PersianCalendar();
            var splited = shamsiDatetime
                .Replace('۱', '1')
                .Replace('۲', '2')
                .Replace('۳', '3')
                .Replace('۴', '4')
                .Replace('۵', '5')
                .Replace('۶', '6')
                .Replace('۷', '7')
                .Replace('۸', '8')
                .Replace('۹', '9')
                .Replace('۰', '0')
                .Split('-').Select(int.Parse).ToArray();

            var res = pc.ToDateTime(splited[0], splited[1], splited[2], 0, 0, 0, 0);

            return DateTime.Parse(shamsiDatetime);
        }

        public static DateTime ToDateTimeWithLanguage(this string datetime, string language)
        {
            var pc = new PersianCalendar();
            if (language == "fa")
            {
                var splited = datetime
                             .Replace('۱', '1')
                             .Replace('۲', '2')
                             .Replace('۳', '3')
                             .Replace('۴', '4')
                             .Replace('۵', '5')
                             .Replace('۶', '6')
                             .Replace('۷', '7')
                             .Replace('۸', '8')
                             .Replace('۹', '9')
                             .Replace('۰', '0')
                             .Split('-').Select(int.Parse).ToArray();

                var res = pc.ToDateTime(splited[0], splited[1], splited[2], 0, 0, 0, 0);
                return res;
            }
            else
            {

                var res = DateTime.Parse(datetime);
                return res;

            }

        }

        public static string ToShamsiDateTime(string Datetime)
        {
            var splited = Datetime
                .Replace('۱', '1')
                .Replace('۲', '2')
                .Replace('۳', '3')
                .Replace('۴', '4')
                .Replace('۵', '5')
                .Replace('۶', '6')
                .Replace('۷', '7')
                .Replace('۸', '8')
                .Replace('۹', '9')
                .Replace('۰', '0')
                .Split('/').Select(int.Parse).ToArray();


            return (splited[0].ToString() + splited[1].ToString() + splited[2].ToString());
        }


        public static string ConvertToEnNum(string str)
        {
            var num = str
                .Replace('۱', '1')
                .Replace('۲', '2')
                .Replace('۳', '3')
                .Replace('۴', '4')
                .Replace('۵', '5')
                .Replace('۶', '6')
                .Replace('۷', '7')
                .Replace('۸', '8')
                .Replace('۹', '9')
                .Replace('۰', '0');


            return (num);
        }



        public static int GetDaysInMonth(int year, int month)
        {
            return new PersianCalendar().GetDaysInMonth(year, month);
        }

        public static DateTime ToMiladiFromShamsi(int year, int month, int day)
        {
            string newDay = day < 10 ? "0" + day : day.ToString();
            var date = $"{year}/{month}/{newDay}";
            return date.ToDateTime();
        }

        public static TimeDurationDataModel GetCurrentTimeDurations(string date, ShowTrends showTrend)
        {
            var currentDate = DateTime.Now;
            var model = new TimeDurationDataModel();
            model.MiladiDate = currentDate;
            model.ShamsiDate = currentDate.ToShamsi();
            model.Year = currentDate.ToShamsiYear();
            model.Month = currentDate.ToShamsiMonth().ToString();
            model.Day = currentDate.ToShamsiDay();
            model.ThreeMonthSeason = currentDate.ToShamsiSeason();
            model.SixMonthSeason = currentDate.ToShamsiSixMonthSeason();

            string month = model.Month.ToString().Length == 1 ? "0" + model.Month : model.Month.ToString();
            string day = model.Day.ToString().Length == 1 ? "0" + model.Day : model.Day.ToString();
            model.MonthDay = $"{month}/{day}";

            if (!string.IsNullOrEmpty(date))
            {
                switch (showTrend)
                {
                    case ShowTrends.Daily:
                        {
                            var array = date.Split("/");
                            var newDate = ToMiladiFromShamsi(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]), Convert.ToInt32(array[2]));
                            model.Day = newDate.ToShamsiDay();
                            var newMonth = newDate.ToShamsiMonth().ToString().Length == 1 ? "0" + newDate.ToShamsiMonth() : newDate.ToShamsiMonth().ToString();
                            var newDay = model.Day.ToString().Length == 1 ? "0" + model.Day : model.Day.ToString();
                            model.MonthDay = $"{newMonth}/{newDay}";
                            model.MiladiDate = newDate;
                            model.ShamsiDate = newDate.ToShamsi();
                            break;
                        }
                    case ShowTrends.Monthly:
                        model.Month = date.Length == 1 ? "0" + date : date;
                        break;
                    case ShowTrends.ThreeMonths:
                        model.ThreeMonthSeason = (Season)Convert.ToInt32(date);
                        break;
                    case ShowTrends.SixMonths:
                        model.SixMonthSeason = Convert.ToInt32(date);
                        break;
                    case ShowTrends.Yearly:
                        model.Year = Convert.ToInt32(date);
                        break;
                    default:
                        break;
                }
            }


            return model;
        }

        public static TimeDurationDataModel GetPreviousPeriodTimeDurations(DateTime specificDate, ShowTrends showTrend)
        {
            var currentDate = specificDate;
            switch (showTrend)
            {
                case ShowTrends.Daily:
                    currentDate = currentDate.AddDays(-1);
                    break;
                case ShowTrends.Monthly:
                    currentDate = currentDate.AddMonths(-1);
                    break;
                case ShowTrends.ThreeMonths:
                    currentDate = currentDate.AddMonths(-3);
                    break;
                case ShowTrends.SixMonths:
                    currentDate = currentDate.AddMonths(-6);
                    break;
                case ShowTrends.Yearly:
                    currentDate = currentDate.AddYears(-1);
                    break;
                default:
                    break;
            }
            var model = new TimeDurationDataModel();
            model.MiladiDate = currentDate;
            model.ShamsiDate = currentDate.ToShamsi();
            model.Year = currentDate.ToShamsiYear();
            model.Month = currentDate.ToShamsiMonth().ToString();
            model.ShamsiMonth = currentDate.ToShamsiMonth();
            model.Day = currentDate.ToShamsiDay();
            model.ThreeMonthSeason = currentDate.ToShamsiSeason();
            model.SixMonthSeason = currentDate.ToShamsiSixMonthSeason();

            string month = model.Month.ToString().Length == 1 ? "0" + model.Month : model.Month.ToString();
            string day = model.Day.ToString().Length == 1 ? "0" + model.Day : model.Day.ToString();
            model.MonthDay = $"{month}/{day}";
            return model;
        }

        public static TimeDurationDataModel GetCurrentTimeDurations()
        {
            var currentDate = DateTime.Now;
            var model = new TimeDurationDataModel();
            model.MiladiDate = currentDate;
            model.ShamsiDate = currentDate.ToShamsi();
            model.Year = currentDate.ToShamsiYear();
            model.Month = currentDate.ToShamsiMonth().ToString();
            model.Day = currentDate.ToShamsiDay();
            model.ThreeMonthSeason = currentDate.ToShamsiSeason();
            model.SixMonthSeason = currentDate.ToShamsiSixMonthSeason();

            string month = model.Month.ToString().Length == 1 ? "0" + model.Month : model.Month.ToString();
            string day = model.Day.ToString().Length == 1 ? "0" + model.Day : model.Day.ToString();
            model.MonthDay = $"{month}/{day}";

            return model;
        }
        public static DateTime? ConvertToMiladi(string date)
        {
            if (!String.IsNullOrEmpty(date))
            {
                DateTime dt = DateTime.Parse(date, new CultureInfo("fa-IR"));
                return dt;
            }
            return null;
        }



        public static TimeDurationDataModel GetSpecificTimeDurations(ShowTrends showTrends, IndexTemplateTimeDurationDataModel duration)
        {
            var currentDate = DateTime.Now;
            var model = new TimeDurationDataModel();

            switch (showTrends)
            {
                case ShowTrends.Daily:
                    model.Day = duration.Value;
                    break;
                case ShowTrends.Monthly:
                    model.Month = duration.Value.ToString().Length == 1 ? "0" + duration.Value.ToString() : duration.Value.ToString();
                    break;
                case ShowTrends.ThreeMonths:
                    model.ThreeMonthSeason = (Season)duration.Value;
                    break;
                case ShowTrends.SixMonths:
                    model.SixMonthSeason = duration.Value;
                    break;
                case ShowTrends.Yearly:
                    model.Year = duration.Value;
                    break;
                default:
                    break;
            }

            model.Year = currentDate.ToShamsiYear();
            return model;
        }

        public static List<IndexTemplateTimeDurationDataModel> GetShamsiYearsDuration(int fromReportYear)
        {
            var result = new List<IndexTemplateTimeDurationDataModel>();
            var currentShamsiYear = DateTime.Now.ToShamsiYear();
            for (int i = fromReportYear; i <= currentShamsiYear; i++)
            {
                result.Add(new IndexTemplateTimeDurationDataModel
                {
                    Text = i.ToString(),
                    Value = i,
                    DateValue = "",
                });
            }
            return result;
        }

        public static List<IndexTemplateTimeDurationDataModel> SixMonthsDuration()
        {
            var result = new List<IndexTemplateTimeDurationDataModel>();

            result.Add(new IndexTemplateTimeDurationDataModel
            {
                Text = "شش ماهه اول",
                Value = 1,
                DateValue = "1",
            });

            result.Add(new IndexTemplateTimeDurationDataModel
            {
                Text = "شش ماهه دوم",
                Value = 2,
                DateValue = "2",
            });

            return result;

        }

        public static List<IndexTemplateTimeDurationDataModel> ThreeMonthsDuration()
        {
            var result = new List<IndexTemplateTimeDurationDataModel>();

            result.Add(new IndexTemplateTimeDurationDataModel
            {
                Text = "سه ماهه اول",
                Value = 1,
                DateValue = "1",
            });

            result.Add(new IndexTemplateTimeDurationDataModel
            {
                Text = "سه ماهه دوم",
                Value = 2,
                DateValue = "2",
            });

            result.Add(new IndexTemplateTimeDurationDataModel
            {
                Text = "سه ماهه سوم",
                Value = 3,
                DateValue = "3",
            });

            result.Add(new IndexTemplateTimeDurationDataModel
            {
                Text = "سه ماهه چهارم",
                Value = 4,
                DateValue = "4",
            });

            return result;
        }

        public static List<IndexTemplateTimeDurationDataModel> MonthsDuration()
        {
            var months = GetShamsiMonths();
            var result = months.Select(q => new IndexTemplateTimeDurationDataModel
            {
                Text = q.Text,
                Value = q.Value,
                DateValue = q.Value <= 9 ? $"0{q.Value}" : q.Value.ToString(),
            }).ToList();

            return result;
        }

        public static List<IndexTemplateTimeDurationDataModel> DaysDuration(int year, int month)
        {
            var result = new List<IndexTemplateTimeDurationDataModel>();
            var days = GetDaysInMonth(year, month);

            for (int i = 1; i <= days; i++)
            {
                string dateValue = year.ToString() + "/";
                dateValue += month <= 9 ? $"0{month}" : month.ToString();
                dateValue += $"/" + (i <= 9 ? $"0{i}" : i.ToString());
                result.Add(new IndexTemplateTimeDurationDataModel
                {
                    Text = $"{year}/{month}/{i}",
                    Value = i,
                    DateValue = dateValue,
                });
            }

            return result;
        }

        public static List<IndexTemplateTimeDurationDataModel> GetDurationTrends(ShowTrends showTrends, int year, int? month)
        {
            var result = new List<IndexTemplateTimeDurationDataModel>();
            switch (showTrends)
            {
                case ShowTrends.Daily:
                    result = DaysDuration(year, month.Value);
                    break;
                case ShowTrends.Monthly:
                    result = MonthsDuration();
                    break;
                case ShowTrends.ThreeMonths:
                    result = ThreeMonthsDuration();
                    break;
                case ShowTrends.SixMonths:
                    result = SixMonthsDuration();
                    break;
                case ShowTrends.Yearly:
                    result = GetShamsiYearsDuration(year);
                    break;
                default:
                    break;
            }

            return result;
        }

        public static DateTime ToMiladiFromString(this string date)
        {
            return Convert.ToDateTime(date);
        }
    }
}
