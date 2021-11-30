using System;

namespace GamesAdmin.Database.Report.Entities
{
    public class SpecialSeDieBetReportEntity : BetReportEntity
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }

        public decimal Refund { get; set; }

        public decimal BetWin { get; set; }

        public string UserId { get; set; }
    }
}