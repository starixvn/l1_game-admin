using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class UnbalancedBigSmallBetHistoryViewModel : BetHistoryViewModel
    {
        public UnbalancedBigSmallRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<UnbalancedBigSmallBetHistoryRecordViewModel> Bets { get; set; }
    }

    public class UnbalancedBigSmallBetHistoryRecordViewModel : BetHistoryRecordViewModel
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}