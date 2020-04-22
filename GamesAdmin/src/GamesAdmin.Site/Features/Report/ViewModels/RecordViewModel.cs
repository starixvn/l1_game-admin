using System;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class RecordViewModel
    {
        public string Username { get; set; }

        public string Nickname { get; set; }

        public bool Select { get; set; }

        public decimal Bet { get; set; }

        public decimal BetWin { get; set; }

        public decimal Back { get; set; }

        public DateTimeOffset Time { get; set; }
    }
}