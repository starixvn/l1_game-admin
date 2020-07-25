using GamesAdmin.Core.Models.BetReport;
using System;

namespace GamesAdmin.Core.Models
{
    public abstract class BetRecord
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public decimal BetWin { get; set; }

        public DateTimeOffset Time { get; set; }

        public GameAccount Account { get; set; }
    }

    public class BigSmallBetRecord : BetRecord
    {
        public bool Select { get; set; }

        public decimal Bet { get; set; }

        public decimal Back { get; set; }
    }

    public class OddEvenBetRecord : BetRecord
    {
        public bool Select { get; set; }

        public decimal Bet { get; set; }

        public decimal Back { get; set; }
    }

    public class SeDieBetRecord : BetRecord
    {
        public decimal Odd { get; set; }

        public decimal Even { get; set; }

        public decimal ThreeRed { get; set; }

        public decimal FourRed { get; set; }

        public decimal ThreeWhite { get; set; }

        public decimal FourWhite { get; set; }
    }
}