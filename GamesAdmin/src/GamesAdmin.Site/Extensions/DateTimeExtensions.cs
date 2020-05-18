using System;

namespace GamesAdmin.Site.Extensions
{
    public static class DateTimeExtensions 
    {
        public static string ToDateAndTimeFormat(this DateTime date)
        {
            if (date == default)
            {
                return string.Empty;
            }

            return date.ToString("dd/MM/yyyy HH:mm");
        }
    }    
}
