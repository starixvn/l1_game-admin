
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class WinThreeCardsBetHistoryViewModel : BetHistoryViewModel
    {
        public WinThreeCardsRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<WinThreeCardsBetHistoryRecordViewModel> Bets { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }

        public decimal GetBetWin( string choice, decimal amount)
        {
            return string.IsNullOrEmpty(choice) ? 0 : Odds[choice] * amount;
        }
    }

    public class WinThreeCardsBetHistoryRecordViewModel : BetHistoryRecordViewModel
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}
