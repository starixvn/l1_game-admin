using System;

namespace GamesAdmin.Database.Report.Entities
{
    public class FishPrawnCrabBetReportEntity : BetReportEntity
    {
        public decimal BetWin { get; set; }

        public decimal Stag { get; set; }

        public decimal Gourd { get; set; }

        public decimal Rooster { get; set; }

        public decimal Fish { get; set; }

        public decimal Crab { get; set; }

        public decimal Prawn { get; set; }
    }
}