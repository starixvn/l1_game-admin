using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BlackjackGameReportResultViewModel : ReportResultViewModel
    {
        public IEnumerable<BlackjackGameRecordViewModel> Records { get; set; }
    }
}