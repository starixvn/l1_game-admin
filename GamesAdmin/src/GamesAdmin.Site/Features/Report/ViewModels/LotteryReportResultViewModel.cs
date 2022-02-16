using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class LotteryReportResultViewModel : ReportResultViewModel
    {
        public LotteryRoundResultViewModel RoundResult { get; set; }
        public IEnumerable<LotteryRecordViewModel> Records { get; set; }
    }
}