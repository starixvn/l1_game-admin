using System;

namespace GamesAdmin.Core.Extensions
{
    public static class DateTimeExtension
    {
        private const int GreenwichTime = -4;
        private const int HourInDay = 24;
        public const int AsianGMT = 7;

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
            return time.AddTicks(-TimeSpan.TicksPerHour * AsianGMT);
        }

        public static int ToGMT4(this int hour)
            => (hour - GreenwichTime + HourInDay) % HourInDay;

        public static string ToStringAtGMT7(this DateTime dateTime)
        {
            const string formatDateTime = "MM/dd/yyyy hh:mm tt";
            return dateTime.AddHours(AsianGMT).ToString(formatDateTime) + " GMT+7";
        }
    }
}