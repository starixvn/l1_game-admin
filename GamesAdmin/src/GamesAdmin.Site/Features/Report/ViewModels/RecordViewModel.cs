namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BetRecordViewModel : BaseBetViewModel
    {
    }

    public class BigSmallRecordViewModel : BetRecordViewModel
    {
        public decimal Bet { get; set; }
        public decimal Back { get; set; }
        public bool Select { get; set; }
    }

    public class OddEvenRecordViewModel : BetRecordViewModel
    {
        public decimal Bet { get; set; }
        public decimal Back { get; set; }
        public bool Select { get; set; }
    }

    public class SeDieRecordViewModel : BetRecordViewModel
    {
        public decimal Odd { get; set; }

        public decimal Even { get; set; }

        public decimal ThreeRed { get; set; }

        public decimal FourRed { get; set; }

        public decimal ThreeWhite { get; set; }

        public decimal FourWhite { get; set; }

        public bool FreeBet { get; set; }
    }

    public class FishPrawnCrabRecordViewModel : BetRecordViewModel
    {
        public decimal Stag { get; set; }

        public decimal Gourd { get; set; }

        public decimal Rooster { get; set; }

        public decimal Fish { get; set; }

        public decimal Crab { get; set; }

        public decimal Prawn { get; set; }
    }

    public class DragonTigerRecordViewModel : BetRecordViewModel
    {
        public decimal Dragon { get; set; }

        public decimal Tie { get; set; }

        public decimal Tiger { get; set; }
    }

    public class BaccaratRecordViewModel : BetRecordViewModel
    {
        public decimal Banker { get; set; }

        public decimal BankerPair { get; set; }

        public decimal Player { get; set; }

        public decimal PlayerPair { get; set; }

        public decimal Tie { get; set; }

        public decimal Big { get; set; }

        public decimal Small { get; set; }
    }

    public class RouletteRecordViewModel : BetRecordViewModel
    {
        public decimal TotalBet { get; set; }
    }
}