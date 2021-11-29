using System;

namespace GamesAdmin.Database.Report.Entities
{
    public class BaccaratBetReportEntity : BetReportEntity
    {
        public decimal BetWin { get; set; }

        public decimal Banker { get; set; }

        public decimal BankerPair { get; set; }

        public decimal Player { get; set; }

        public decimal PlayerPair { get; set; }

        public decimal Tie { get; set; }

        public decimal Big { get; set; }

        public decimal Small { get; set; }
    }
}