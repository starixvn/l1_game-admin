using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class WinThreeCardsBetHistoryViewModel : BetHistoryViewModel
    {
        public WinThreeCardsRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<WinThreeCardsBetHistoryRecordViewModel> Bets { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
    }

    public class WinThreeCardsBetHistoryRecordViewModel : BetHistoryRecordViewModel
    {
        private readonly Dictionary<string, string> ListChoices = new Dictionary<string, string>()
        {
            ["dragon"] = "Dragon",
            ["phoenix"] = "Phoenix",
            ["pair8Plus"] = "Pair 8 Plus",
            ["straight"] = "Straight",
            ["flush"] = "Flush",
            ["straightFlush"] = "Straight Flush",
            ["threeOfAKind"] = "Three of a Kind",
        };

        public string DisplayChoice => ListChoices[Choice];
        public string Choice { get; set; }
        public decimal Amount { get; set; }
    }
}