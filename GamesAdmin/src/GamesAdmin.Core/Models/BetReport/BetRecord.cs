using System;

namespace GamesAdmin.Core.Models
{
    public class BetRecord
    {
        public string Username { get; set; }

        public string Nickname { get; set; }

        public decimal BetWin { get; set; }

        public DateTimeOffset Time { get; set; }
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
}