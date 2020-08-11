using System;

namespace GamesAdmin.Database.Report.Entities
{
    public class RouletteBetReportEntity
    {
        public DateTime Time { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public decimal BetWin { get; set; }

        public decimal TotalBet { get; set; }
    }
}