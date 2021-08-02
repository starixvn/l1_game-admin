using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Site.Extensions;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class VietnameseHeroesRecordViewModel : BetRecordViewModel
    {
        public long RoundId { get; set; }

        public decimal Bet { get; set; }

        public decimal TotalAmount { get; set; }

        public IEnumerable<byte> Paylines { get; set; }

        public bool FreeSpin { get; set; }

        public IEnumerable<byte> Result { get; set; }

        public VietnameseHeroesSettlementResultViewModel SettlementResult { get; set; }

        public IEnumerable<VietnameseHeroesBonusChestViewModel> BonusChests { get; set; }

        public bool Paid { get; set; }

        public byte[][] GetFormatResults() => Result.ToArray().FormatBoard();

        public decimal TotalBetWin => BetWin + (SettlementResult.Jackpot?.WinningAmount ?? 0);

        public decimal WinLoss => FreeSpin ? TotalBetWin : TotalBetWin - TotalAmount;
    }

    public class VietnameseHeroesBonusChestViewModel
    {
        public decimal Amount { get; set; }

        public bool Picked { get; set; }
    }

    public class VietnameseHeroesSettlementResultViewModel
    {
        public IEnumerable<VietnameseHeroesWinningLineViewModel> WinningPaylines { get; set; }

        public VietnameseHeroesBonusViewModel Bonus { get; set; }

        public VietnameseHeroesFreeSpinViewModel FreeSpins { get; set; }

        public VietnameseHeroesJackpotViewModel Jackpot { get; set; }
    }

    public class VietnameseHeroesJackpotViewModel
    {
        public byte WinningLine { get; set; }

        public decimal WinningAmount { get; set; }
    }

    public class VietnameseHeroesWinningLineViewModel
    {
        public byte Payline { get; set; }

        public string Symbol { get; set; }

        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class VietnameseHeroesBonusViewModel
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class VietnameseHeroesFreeSpinViewModel
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }
}