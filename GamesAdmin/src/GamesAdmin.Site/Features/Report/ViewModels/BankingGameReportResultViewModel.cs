using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BankingGameReportResultViewModel : ReportResultViewModel
    {
        public IEnumerable<BankingGameRecordViewModel> Records { get; set; }
    }
}