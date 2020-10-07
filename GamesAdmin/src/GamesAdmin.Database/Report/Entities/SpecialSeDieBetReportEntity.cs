using System;

namespace GamesAdmin.Database.Report.Entities
{
    public class SpecialSeDieBetReportEntity
    {
        public DateTime Time { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }

        public decimal Refund { get; set; }

        public decimal BetWin { get; set; }

        public string UserId { get; set; }
    }
}
