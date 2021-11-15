using System;

namespace GamesAdmin.Database.Report.Entities
{
    public class CasinoHoldemPokerBetReportEntity
    {
        public DateTime Time { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public decimal BetWin { get; set; }

        public decimal Ante { get; set; }

        public decimal AA { get; set; }

        public decimal Call { get; set; }
    }
}