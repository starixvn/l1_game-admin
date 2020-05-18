using System;
using System.ComponentModel;
using GamesAdmin.Site._Shared.Configurations;

namespace GamesAdmin.Site.Features.UmSchedule.ViewModels
{
    public class AddViewModel
    {
        public AddViewModel(IAppSettings appSettings) 
        {
            Start = DateTime.Now;
            End = DateTime.Now.AddMinutes(30);
            Message = string.Format(appSettings.DefaultUmMessage, StartDisplay, EndDisplay);
        }

        [DisplayName("Start From")]
        public DateTime Start { get; set; }

        public string StartDisplay => Start.ToString("dd/MM/yyyy HH:mm");

        [DisplayName("End At")]
        public DateTime End { get; set; }

        public string EndDisplay => End.ToString("dd/MM/yyyy HH:mm");

        [DisplayName("Under Maintenance Message")]
        public string Message { get; set; }

        public string ErrorMessage { get; set; }
    }
}
