using System;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BigSmallRecordViewModel
    {
        public string Username { get; set; }

        public string Nickname { get; set; }

        public bool SelectBig { get; set; }

        public decimal Bet { get; set; }

        public decimal BetWin { get; set; }

        public decimal Back { get; set; }

        public DateTime Time { get; set; }
    }
}