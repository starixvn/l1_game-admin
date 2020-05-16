using System;

namespace GamesAdmin.Site.Features.UmSchedule.ViewModels
{
    public class AddViewModel
    {
        public AddViewModel() 
        {
            From = DateTime.Now;
            To = DateTime.Now.AddMinutes(30);
        }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string Message { get; set; }

        public string ErrorMessage { get; set; }
    }
}
