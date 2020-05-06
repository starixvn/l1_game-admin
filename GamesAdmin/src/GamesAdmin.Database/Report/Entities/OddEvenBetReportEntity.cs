using System;

namespace GamesAdmin.Database.Report.Entities
{
    public class OddEvenBetReportEntity
    {
        public DateTime Time { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public bool Select { get; set; }

        public decimal Bet { get; set; }

        public decimal BetWin { get; set; }

        public decimal Back { get; set; }
    }
}