using System;
using System.ComponentModel;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class UnderMaintenanceViewModel
    {
        public UnderMaintenanceViewModel()
        {
            var current = DateTime.Now;
            StartTime = current.AddSeconds(-current.Second).AddMilliseconds(-current.Millisecond);
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
