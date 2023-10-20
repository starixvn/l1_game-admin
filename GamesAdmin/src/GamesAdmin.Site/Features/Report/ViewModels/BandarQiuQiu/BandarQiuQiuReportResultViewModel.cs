using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BandarQiuQiuReportResultViewModel : ReportResultViewModel
    {
        public BandarQiuQiuRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<BandarQiuQiuRecordViewModel> Records { get; set; }
    }
}