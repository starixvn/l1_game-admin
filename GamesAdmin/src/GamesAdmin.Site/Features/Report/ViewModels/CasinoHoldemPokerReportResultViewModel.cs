using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CasinoHoldemPokerReportResultViewModel
    {
        public CasinoHoldemPokerRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<CasinoHoldemPokerRecordViewModel> Records { get; set; }
    }
}