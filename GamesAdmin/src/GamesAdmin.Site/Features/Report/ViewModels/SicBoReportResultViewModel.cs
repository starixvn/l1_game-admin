using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class SicBoReportResultViewModel
    {
        public SicBoRoundResultViewModel RoundResult { get; set; }
        public IEnumerable<SicBoRecordViewModel> Records { get; set; }
    }
}