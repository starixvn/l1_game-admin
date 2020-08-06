using System;
using System.ComponentModel;
using GamesAdmin.Site._Shared.Configurations;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class UnderMaintenanceViewModel
    {
        public UnderMaintenanceViewModel(IAppSettings appSettings)
        {            
            var current = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, appSettings.DefaultTimeZone);
            var dateTimeWithTz = new DateTimeOffset(current.Year, current.Month, current.Day, current.Hour, current.Minute, 0, new TimeSpan(7, 0, 0));

            StartTime = dateTimeWithTz;
            EndTime = StartTime.AddHours(1);
            IsUM = true;
        }

        [DisplayName("Start Time (GMT+7)")]
        public DateTimeOffset StartTime { get; set; }

        [DisplayName("End Time (GMT+7)")]
        public DateTimeOffset EndTime { get; set; }

        [DisplayName("Is Under Maintenance")]
        public bool IsUM { get; set; }
    }
}
