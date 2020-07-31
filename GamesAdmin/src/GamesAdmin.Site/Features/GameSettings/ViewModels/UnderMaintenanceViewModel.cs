using System;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class UnderMaintenanceViewModel
    {
        public UnderMaintenanceViewModel()
        {
            StartTime = DateTime.Now;
            EndTime = StartTime.AddHours(1);
            IsUM = true;
        }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public bool IsUM { get; set; }
    }
}
