using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Site.Extensions;

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

        public decimal TwoRed { get; set; }

        public decimal ThreeRed { get; set; }

        public decimal FourRed { get; set; }

        public decimal ThreeWhite { get; set; }

        public decimal FourWhite { get; set; }

        public bool FreeBet { get; set; }
    }

    public class VipSeDieRecordViewModel : BetRecordViewModel
    {
        public decimal Odd { get; set; }

        public decimal Even { get; set; }

        public decimal TwoRed { get; set; }

        public decimal ThreeRed { get; set; }

        public decimal FourRed { get; set; }

        public decimal ThreeWhite { get; set; }

        public decimal FourWhite { get; set; }
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

    public class BlackjackRecordViewModel : BetRecordViewModel
    {
        public long HandId { get; set; }

        public decimal Bet { get; set; }

        public long? SplitForm { get; set; }

        public long Round { get; set; }

        public IEnumerable<BlackjackCardViewModel> Cards { get; set; }

        public IEnumerable<string> Actions { get; set; }

        public IEnumerable<BlackjackCardViewModel> DealerCards { get; set; }

        public bool Ended { get; set; }

        public byte Point { get; set; }

        public byte DealerPoint { get; set; }

        public IEnumerable<byte> SoftPoints { get; set; }
    }

    public class BlackjackCardViewModel
    {
        public string Rank { get; set; }

        public byte Suit { get; set; }
    }

    public class SpecialSeDieRecordViewModel : BetRecordViewModel
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }

        public decimal Refund { get; set; }
    }

    public class VipFishPrawnCrabRecordViewModel : BetRecordViewModel
    {
        public decimal Stag { get; set; }

        public decimal Gourd { get; set; }

        public decimal Rooster { get; set; }

        public decimal Fish { get; set; }

        public decimal Crab { get; set; }

        public decimal Prawn { get; set; }

        public decimal AnyTriple { get; set; }

        public decimal Mountain { get; set; }

        public decimal Ocean { get; set; }
    }

    public class SicBoRecordViewModel : BetRecordViewModel
    {
        public decimal TotalBet { get; set; }
    }
    
    public class VipSeDieWarRecordViewModel : BetRecordViewModel
    {
        public decimal TotalBet { get; set; }
    }

    public class FourGuardiansRecordViewModel : BetRecordViewModel
    {
        public long RoundId { get; set; }

        public decimal Bet { get; set; }

        public decimal TotalAmount { get; set; }

        public IEnumerable<byte> Paylines { get; set; }

        public bool FreeSpin { get; set; }

        public IEnumerable<byte> Result { get; set; }

        public FourGuardiansSettlementResultViewModel SettlementResult { get; set; }

        public IEnumerable<FourGuardiansBonusChestViewModel> BonusChests { get; set; }

        public bool Paid { get; set; }

        public byte[,] GetFormatResults() => Result.ToArray().FormatBoard();
    }

    public class FourGuardiansBonusChestViewModel
    {
        public decimal Amount { get; set; }

        public bool Picked { get; set; }
    }

    public class FourGuardiansSettlementResultViewModel
    {
        public IEnumerable<FourGuardiansWinningLineViewModel> WinningPaylines { get; set; }

        public FourGuardiansBonusViewModel Bonus { get; set; }

        public FourGuardiansFreeSpinViewModel FreeSpins { get; set; }

        public FourGuardiansJackpotViewModel Jackpot { get; set; }
    }

    public class FourGuardiansJackpotViewModel
    {
        public byte WinningLine { get; set; }

        public decimal WinningAmount { get; set; }
    }

    public class FourGuardiansWinningLineViewModel
    {
        public byte Payline { get; set; }

        public string Symbol { get; set; }

        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class FourGuardiansBonusViewModel
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class FourGuardiansFreeSpinViewModel
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }
}