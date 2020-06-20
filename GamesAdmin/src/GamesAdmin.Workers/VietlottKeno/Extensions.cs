using System;

namespace GamesAdmin.Workers.VietlottKeno
{
    public static class Extensions
    {
        public static DateTime CalculateNextRoundTime(this DateTime time, long roundDistance = 1)
        {
            var startTimeOfDay = VietlottConstants.StartTimeOfDayUtc;
            var endTimeOfDay = VietlottConstants.EndTimeOfDayUtc;

            var nextRoundTime = time;

            for (var i = 1; i <= roundDistance; i++)
            {
                nextRoundTime = nextRoundTime.AddMinutes(10);
                if (nextRoundTime.TimeOfDay > endTimeOfDay && nextRoundTime.TimeOfDay < startTimeOfDay)
                {
                    nextRoundTime = new DateTime(time.Year, time.Month, time.Day, startTimeOfDay.Hours, 10, 0, DateTimeKind.Utc);
                }
            }

            return nextRoundTime;
        }
    }
}