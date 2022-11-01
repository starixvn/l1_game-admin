

using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class Win3CardsBetHistoryViewModel : BetHistoryViewModel
    {
        public Win3CardsRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<Win3CardsBetHistoryRecordViewModel> Bets { get; set; }
    }

    public class Win3CardsBetHistoryRecordViewModel : BetHistoryRecordViewModel
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}
