using System.Collections.Generic;
using GamesAdmin.Core.Enumeration.BetChoices;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoBetHistoryViewModel : BetHistoryViewModel
    {
        public IEnumerable<KenoBetHistoryRecordViewModel> Bets { get; set; }

        public int BaseCurrencyMultiplier { get; set; }

    }

    public class KenoBetHistoryRecordViewModel : BetHistoryRecordViewModel
    {
        public KenoBetChoice Choice { get; set; }

        public decimal Amount { get; set; }
    }
}
