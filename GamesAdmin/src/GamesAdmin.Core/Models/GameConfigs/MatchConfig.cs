using System;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class MatchConfig
    {
        public string Id { get; set; }
        public string Home { get; set; }
        public string Away { get; set; }
        public DateTime StartTime { get; set; }

        public void ConvertToTimeZone(TimeZoneInfo timeZone)
        {
            StartTime = TimeZoneInfo.ConvertTimeFromUtc(StartTime, timeZone);
        }
    }
}