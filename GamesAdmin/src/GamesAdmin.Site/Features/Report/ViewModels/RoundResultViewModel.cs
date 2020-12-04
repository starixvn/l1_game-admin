using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class RoundResultViewModel
    {
        public long Number { get; set; }

        public int Dice1 { get; set; }

        public int Dice2 { get; set; }

        public int Dice3 { get; set; }

        public int Total => Dice1 + Dice2 + Dice3;

        public bool IsBig => Total > 10;

        public bool IsEven => Total % 2 == 0;
    }

    public class SeDieRoundResultViewModel
    {
        public long Number { get; set; }

        public IEnumerable<bool> Chips { get; set; }

        public int TotalRed => Chips.Count(x => x);

        public bool Even { get; set; }

        public bool Odd { get; set; }

        public bool TwoRed { get; set; }

        public bool ThreeRed { get; set; }

        public bool FourRed { get; set; }

        public bool ThreeWhite { get; set; }

        public bool FourWhite { get; set; }
    }

    public class FishPrawnCrabRoundResultViewModel
    {
        public long Number { get; set; }

        public byte FirstDice { get; set; }

        public byte SecondDice { get; set; }

        public byte ThirdDice { get; set; }
    }

    public class DragonTigerRoundResultViewModel
    {
        public long Number { get; set; }

        public CardViewModel Dragon { get; set; }

        public CardViewModel Tiger { get; set; }
    }

    public class CardViewModel
    {
        public byte Number { get; set; }

        public byte Type { get; set; }
    }

    public class BaccaratRoundResultViewModel
    {
        public long Number { get; set; }

        public IEnumerable<CardViewModel> PlayerCards { get; set; }

        public IEnumerable<CardViewModel> BankerCards { get; set; }

        public BaccaratSettlementResultViewModel Result { get; set; }
    }

    public class VipBaccaratRoundResultViewModel
    {
        public long Number { get; set; }

        public VipBaccaratResultViewModel Result { get; set; }

        public BaccaratSettlementResultViewModel SettlementResult { get; set; }
    }

    public class VipBaccaratResultViewModel
    {
        public IEnumerable<CardViewModel> Player { get; set; }

        public IEnumerable<CardViewModel> Banker { get; set; }
    }

    public class BaccaratSettlementResultViewModel
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

    public class RouletteRoundResultViewModel
    {
        public long Number { get; set; }

        public byte Result { get; set; }

        public RouletteSettlementResultViewModel SettlementResult { get; set; }
    }

    public class RouletteSettlementResultViewModel
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

    public class VipFishPrawnCrabRoundResultViewModel
    {
        public long Number { get; set; }

        public IEnumerable<byte> Result { get; set; }
    }

    public class SicBoRoundResultViewModel
    {
        public long Number { get; set; }

        public IEnumerable<byte> Result { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }
}