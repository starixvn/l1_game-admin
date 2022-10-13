using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class FanTanReportResultViewModel : ReportResultViewModel
    {
        public FanTanRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<FanTanRecordViewModel> Records { get; set; }
    }
}