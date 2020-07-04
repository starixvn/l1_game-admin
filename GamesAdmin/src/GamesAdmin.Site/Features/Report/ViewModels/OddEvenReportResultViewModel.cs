using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class OddEvenReportResultViewModel
    {
        public RoundResultViewModel RoundResult { get; set; }

        public IEnumerable<OddEvenRecordViewModel> Records { get; set; }
    }
}