using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class RoundResult
    {
        public long Number { get; set; }
    }

    public class BigSmallRoundResult : RoundResult
    {
        public int Dice1 { get; set; }

        public int Dice2 { get; set; }

        public int Dice3 { get; set; }
    }

    public class OddEvenRoundResult : RoundResult
    {
        public int Dice1 { get; set; }

        public int Dice2 { get; set; }

        public int Dice3 { get; set; }
    }

    public class SeDieRoundResult : RoundResult
    {
        public IEnumerable<bool> Chips { get; set; }
    }

    public class FishPrawnCrabRoundResult : RoundResult
    {
        public byte FirstDice { get; set; }

        public byte SecondDice { get; set; }

        public byte ThirdDice { get; set; }
    }
}