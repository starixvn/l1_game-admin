using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BlackjackReportResultViewModel : ReportResultViewModel
    {
        public bool ConvertToUSD { get; set; }
        public IEnumerable<BlackjackRecordViewModel> Records { get; set; }
    }
}