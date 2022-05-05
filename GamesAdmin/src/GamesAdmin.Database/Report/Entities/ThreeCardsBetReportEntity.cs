using System;

namespace GamesAdmin.Database.Report.Entities
{
    public class ThreeCardsBetReportEntity : BetReportEntity
    {
        public decimal Red { get; set; }

        public decimal Black { get; set; }

        public decimal Diamonds { get; set; }

        public decimal Hearts { get; set; }

        public decimal Spades { get; set; }

        public decimal Clubs { get; set; }

        public decimal BetWin { get; set; }
    }
}
