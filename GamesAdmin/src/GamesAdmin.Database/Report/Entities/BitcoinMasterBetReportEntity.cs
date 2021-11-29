using System;

namespace GamesAdmin.Database.Report.Entities
{
    public class BitcoinMasterBetReportEntity : BetReportEntity
    {

        public decimal Bet { get; set; }

        public decimal BetWin { get; set; }

        public decimal Back { get; set; }

        public bool Win { get; set; }

        public string Choice { get; set; }
    }
}