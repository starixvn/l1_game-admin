using System;

namespace GamesAdmin.Database.Report.Entities
{
    public class SeDieBetReportEntity
    {
        public DateTime Time { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public decimal Odd { get; set; }

        public decimal Even { get; set; }

        public decimal ThreeRed { get; set; }

        public decimal FourRed { get; set; }

        public decimal ThreeWhite { get; set; }

        public decimal FourWhite { get; set; }

        public decimal BetWin { get; set; }

        public bool FreeBet { get; set; }
    }
}
