using System.Collections.Generic;
using GamesAdmin.Core.Models.BetReport;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoBetHistoryViewModel
    {
        public KenoBetHistoryViewModel(IEnumerable<KenoBetHistory> betHistories)
        {
            BetHistories = betHistories;
        }

        public IEnumerable<KenoBetHistory> BetHistories { get; }
    }
}
