using System;

namespace GamesAdmin.Database.Report.Entities
{
    public class VipFishPrawnCrabBetReportEntity
    {
        public DateTime Time { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public decimal BetWin { get; set; }

        public decimal Stag { get; set; }

        public decimal Gourd { get; set; }

        public decimal Rooster { get; set; }

        public decimal Fish { get; set; }

        public decimal Crab { get; set; }

        public decimal Prawn { get; set; }

        public decimal AnyTriple { get; set; }
    }
}