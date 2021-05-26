using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class ThreeCardsReportResultViewModel
    {
        public ThreeCardsRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<ThreeCardsRecordViewModel> Records { get; set; }
    }
}