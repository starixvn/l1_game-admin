using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Extensions;

namespace GamesAdmin.Site.Features.UmSchedule.ViewModels
{
    public class AddViewModel
    {
        public AddViewModel(IAppSettings appSettings) 
        {
            Start = DateTimeOffset.Now;
            End = DateTimeOffset.Now.AddMinutes(30);
            Message = string.Format(appSettings.DefaultUmMessage, StartDisplay, EndDisplay);
        }

        [DisplayName("Start From")]
        [Required]
        public DateTimeOffset Start { get; set; }

        public string StartDisplay => Start.ToDateAndTimeFormat();

        [DisplayName("End At")]
        [Required]
        public DateTimeOffset End { get; set; }

        public string EndDisplay => End.ToDateAndTimeFormat();

        [DisplayName("Under Maintenance Message")]
        [Required]
        public string Message { get; set; }

        public string ErrorMessage { get; set; }
    }
}
