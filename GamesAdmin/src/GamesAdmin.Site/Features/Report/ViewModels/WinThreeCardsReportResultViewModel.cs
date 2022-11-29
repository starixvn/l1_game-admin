using System.Collections.Generic;


namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class WinThreeCardsReportResultViewModel : ReportResultViewModel
    {
        public WinThreeCardsRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<WinThreeCardsRecordViewModel> Records { get; set; }
    }
}
