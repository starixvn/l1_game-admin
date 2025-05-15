using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CrashTurnGameReportResultViewModel : ReportResultViewModel
    {
        public IEnumerable<CrashTurnGameRecordViewModel> Records { get; set; }
    }
}
