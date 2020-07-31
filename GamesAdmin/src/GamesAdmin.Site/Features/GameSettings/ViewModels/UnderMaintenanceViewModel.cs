using System;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class UnderMaintenanceViewModel
    {
        public UnderMaintenanceViewModel()
        {
            From = DateTime.Now;
            To = From.AddHours(1);
            IsUM = true;
        }

        public DateTimeOffset From { get; set; }

        public DateTimeOffset To { get; set; }

        public bool IsUM { get; set; }
    }
}
