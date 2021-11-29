using System;

namespace GamesAdmin.Database.Report.Entities
{
    public class SpecialSeDieBetReportEntity : BetReportEntity
    {
        public DateTime Time { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }

        public decimal Refund { get; set; }

        public decimal BetWin { get; set; }

        public string UserId { get; set; }

        public string Currency { get; set; }

        public string Partner { get; set; }

        public decimal Rate { get; set; }
    }
}