using System;

namespace GamesAdmin.Site.Extensions
{
    public static class DateTimeExtensions
    {
        public const string ReportDateTimeFormat = "dd/MM/yyyy HH:mm:ss";
        private const int GreenwichTime = -4;
        private const int AsiaTime = 7;

        public static string ToDateAndTimeFormat(this DateTimeOffset date)
        {
            if (date == default)
            {
                return string.Empty;
            }

            return date.ToString("yyyy-M-dd HH:mm");
        }

        public static string ToReportDateTimeFormat(this DateTime date, TimeZoneInfo timezone)
        {
            if (date == default)
            {
                return string.Empty;
            }

            return TimeZoneInfo.ConvertTime(date, timezone).ToString(ReportDateTimeFormat);
        }

        public static int ToGreenwichHour(this DateTime date)
        => date.AddHours(GreenwichTime).Hour;

        public static int ToAsiaHour(this DateTime date)
        => date.AddHours(AsiaTime).Hour;
    }
}
