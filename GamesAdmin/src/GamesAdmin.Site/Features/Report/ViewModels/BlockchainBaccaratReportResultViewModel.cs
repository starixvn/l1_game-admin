using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BlockchainBaccaratReportResultViewModel : ReportResultViewModel
    {
        public BaccaratRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<BaccaratRecordViewModel> Records { get; set; }
    }
}
