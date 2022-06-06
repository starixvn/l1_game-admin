using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    // TO DO: reuse this viewmodels instead of duplicate code
    public class BaccaratReportResultViewModel : ReportResultViewModel
    {
        public BaccaratRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<BaccaratRecordViewModel> Records { get; set; }
    }
}