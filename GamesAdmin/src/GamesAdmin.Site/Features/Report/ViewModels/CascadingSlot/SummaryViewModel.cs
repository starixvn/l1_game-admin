using GamesAdmin.Core;
using GamesAdmin.Site.Features._Shared.Helpers;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class SummaryViewModel
    {
        public SummaryViewModel(CascadingSlotRecordViewModel round)
        {
            Round = round;
        }

        public CascadingSlotRecordViewModel Round { get; set; }

        public string FreeSpinsLabel => Round.FreeSpin ? "Free Spins" : string.Empty;
        public string BuyFreeSpinsLabel => Round.BuyFreeSpin? "Buy Free Spins" : string.Empty;

        public string DisplayBetWin => HtmlHelpers.formatNumber(Round.BetWin);

        public string DisplayCoinValue => HtmlHelpers.formatNumber(Round.CoinValue);

        public string DisplayWinLoss => HtmlHelpers.formatNumber(Round.WinLoss);

        public string DisplayTime => Round.Time.ToString(Constants.DateTimeFormat);

        public decimal TotalAmount => Round.FreeSpin ? 0 : Round.TotalAmount;

        public string DisplayTotalAmount => HtmlHelpers.formatNumber(Round.TotalAmount);

        public string DisplayTotalAmountWithFreeSpin => HtmlHelpers.formatNumber(TotalAmount);
    }
}
