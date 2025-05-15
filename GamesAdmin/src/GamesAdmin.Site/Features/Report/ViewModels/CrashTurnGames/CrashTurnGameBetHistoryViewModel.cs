using GamesAdmin.Site.Features._Shared.Helpers;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CrashTurnGameBetHistoryViewModel : BetHistoryRecordViewModel
    {
        public decimal CurrencyMultiplier { get; set; }

        private decimal baseWinAmount { get; set; }

        public decimal WinAmount
        {
            get => baseWinAmount * CurrencyMultiplier;
            set
            {
                baseWinAmount = value;
            }
        }

        private decimal baseAmount { get; set; }

        public decimal Amount
        {
            get => baseAmount * CurrencyMultiplier;
            set
            {
                baseAmount = value;
            }
        }

        public decimal Multiplier { get; set; }
        public string Action { get; set; }

        public string DisplayMultiplier => Multiplier > 0 ? $"{HtmlHelpers.formatNumber(Multiplier)}x" : "-";
    }
}
