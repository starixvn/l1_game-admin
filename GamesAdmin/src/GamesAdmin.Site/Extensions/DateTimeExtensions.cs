using System;

namespace GamesAdmin.Site.Extensions
{
    public static class DateTimeExtensions 
    {
        public static string ToDateAndTimeFormat(this DateTimeOffset date)
        {
            if (date == default)
            {
                return string.Empty;
            }

            return date.ToString("yyyy-M-dd HH:mm");
        }
    }    
}
