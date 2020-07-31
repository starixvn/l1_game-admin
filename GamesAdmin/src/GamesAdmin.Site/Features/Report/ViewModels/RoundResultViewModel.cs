using System.Collections.Generic;

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
}