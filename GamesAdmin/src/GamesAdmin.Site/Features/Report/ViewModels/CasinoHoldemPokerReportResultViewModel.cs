using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CasinoHoldemPokerReportResultViewModel
    {
        public bool ConvertToUSD { get; set; }

        public IEnumerable<CasinoHoldemPokerRecordViewModel> Records { get; set; }
    }
}