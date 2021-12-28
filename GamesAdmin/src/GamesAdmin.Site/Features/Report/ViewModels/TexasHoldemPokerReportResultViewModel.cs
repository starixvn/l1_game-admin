using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class TexasHoldemPokerReportResultViewModel
    {
        public bool ConvertToUSD { get; set; }

        public IEnumerable<TexasHoldemPokerRecordViewModel> Records { get; set; }

    }
}
