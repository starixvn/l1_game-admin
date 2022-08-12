using GamesAdmin.Site._Shared.Configurations;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class VipBaccaratReportResultViewModel : ReportResultViewModel
    {

        public BaccaratRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<BaccaratRecordViewModel> Records { get; set; }
    }
}
