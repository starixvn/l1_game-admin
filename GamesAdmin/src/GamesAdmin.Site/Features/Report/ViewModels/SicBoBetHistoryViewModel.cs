using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class SicBoBetHistoryViewModel : BetHistoryViewModel
    {
        public SicBoRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<SicBoBetHistoryRecordViewModel> Bets { get; set; }
    }

    public class SicBoBetHistoryRecordViewModel : BetHistoryRecordViewModel
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}