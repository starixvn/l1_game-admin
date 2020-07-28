using System;

namespace GamesAdmin.Core.Models
{
    public abstract class BetRecord
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public decimal BetWin { get; set; }

        public DateTime Time { get; set; }

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

    public class FishPrawnCrabBetRecord : BetRecord
    {
        public decimal Stag { get; set; }

        public decimal Gourd { get; set; }

        public decimal Rooster { get; set; }

        public decimal Fish { get; set; }

        public decimal Crab { get; set; }

        public decimal Prawn { get; set; }
    }

    public class DragonTigerBetRecord : BetRecord
    {
        public decimal Dragon { get; set; }

        public decimal Tie { get; set; }

        public decimal Tiger { get; set; }
    }
}