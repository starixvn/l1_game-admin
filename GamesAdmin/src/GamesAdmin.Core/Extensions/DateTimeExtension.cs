using System;

namespace GamesAdmin.Core.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ToStartDate(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        }

        public static DateTime ToEndDate(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
        }

        public static DateTime ToGMT7(this DateTime time)
        {
            const int gmt = 7;
            return time.AddTicks(-TimeSpan.TicksPerHour * gmt);
        }
    }
}
