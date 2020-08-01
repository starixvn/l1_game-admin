using System;

namespace GamesAdmin.Site.Extensions
{
    public static class DateTimeExtensions
    {
        public const string ReportDateTimeFormat = "dd/MM/yyyy HH:mm:ss";

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
    }
}
