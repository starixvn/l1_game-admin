using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Site.Extensions;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class HeroesSlotRecordViewModel : BetRecordViewModel
    {
        public long RoundId { get; set; }

        public decimal Bet { get; set; }

        public decimal TotalAmount { get; set; }

        public IEnumerable<byte> Paylines { get; set; }

        public bool FreeSpin { get; set; }

        public IEnumerable<byte> Result { get; set; }

        public HeroesSlotSettlementResultViewModel SettlementResult { get; set; }

        public IEnumerable<HeroesSlotBonusChestViewModel> BonusChests { get; set; }

        public bool Paid { get; set; }

        public byte[][] GetFormatResults() => Result.ToArray().FormatBoard();

        public decimal TotalBetWin => BetWin;

        public decimal WinLoss => FreeSpin ? TotalBetWin : TotalBetWin - TotalAmount;
    }

    public class HeroesSlotBonusChestViewModel
    {
        public decimal Amount { get; set; }

        public bool Picked { get; set; }
    }

    public class HeroesSlotSettlementResultViewModel
    {
        public IEnumerable<HeroesSlotWinningLineViewModel> WinningPaylines { get; set; }

        public HeroesSlotBonusViewModel Bonus { get; set; }

        public HeroesSlotFreeSpinViewModel FreeSpins { get; set; }
    }

    public class HeroesSlotWinningLineViewModel
    {
        public byte Payline { get; set; }

        public string Symbol { get; set; }

        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class HeroesSlotBonusViewModel
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class HeroesSlotFreeSpinViewModel
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }
}