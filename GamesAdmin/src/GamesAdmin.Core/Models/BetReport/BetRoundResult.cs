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

        public bool Odd { get; set; }

        public bool Even { get; set; }

        public bool TwoRed { get; set; }

        public bool ThreeRed { get; set; }

        public bool FourRed { get; set; }

        public bool ThreeWhite { get; set; }

        public bool FourWhite { get; set; }
    }

    public class FishPrawnCrabRoundResult : RoundResult
    {
        public byte FirstDice { get; set; }

        public byte SecondDice { get; set; }

        public byte ThirdDice { get; set; }
    }

    public class DragonTigerRoundResult : RoundResult
    {
        public CardResult Dragon { get; set; }

        public CardResult Tiger { get; set; }
    }

    public class CardResult
    {
        public byte Number { get; set; }

        public byte Type { get; set; }
    }

    public class BaccaratRoundResult : RoundResult
    {
        public IEnumerable<CardResult> PlayerCards { get; set; }

        public IEnumerable<CardResult> BankerCards { get; set; }

        public BaccaratSettlementResult Result { get; set; }
    }

    public class BaccaratSettlementResult
    {
        public bool Player { get; set; }
        public bool Banker { get; set; }
        public bool Tie { get; set; }
        public bool Big { get; set; }
        public bool Small { get; set; }
        public bool PlayerPair { get; set; }
        public bool BankerPair { get; set; }
        public bool Natural { get; set; }
    }

    public class RouletteRoundResult : RoundResult
    {
        public byte Result { get; set; }

        public RouletteSettlementResult SettlementResult { get; set; }
    }

    public class RouletteSettlementResult
    {
        public IEnumerable<string> StraightUp { get; set; }

        public IEnumerable<string> Split { get; set; }

        public IEnumerable<string> Street { get; set; }

        public IEnumerable<string> Triangle { get; set; }

        public IEnumerable<string> Corner { get; set; }

        public IEnumerable<string> FourNumbers { get; set; }

        public IEnumerable<string> Line { get; set; }

        public IEnumerable<string> Column { get; set; }

        public IEnumerable<string> Dozen { get; set; }

        public IEnumerable<string> RedBlack { get; set; }

        public IEnumerable<string> OddEven { get; set; }

        public IEnumerable<string> BigSmall { get; set; }
    }

    public class VipFishPrawnCrabRoundResult : RoundResult
    {
        public byte FirstDice { get; set; }

        public byte SecondDice { get; set; }

        public byte ThirdDice { get; set; }
    }
}