using System.Collections.Generic;


namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class Win3CardsReportResultViewModel : ReportResultViewModel
    {
        public Win3CardsRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<Win3CardsRecordViewModel> Records { get; set; }
    }
}
