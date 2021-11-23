using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BlackjackReportResultViewModel
    {
        public bool ConvertToUSD { get; set; }
        public IEnumerable<BlackjackRecordViewModel> Records { get; set; }
    }
}