using System;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BaseBetViewModel
    {
        public string Username { get; set; }

        public string Nickname { get; set; }

        public decimal BetWin { get; set; }

        public DateTimeOffset Time { get; set; }
    }
}