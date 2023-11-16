using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CrashGameReportResultViewModel : ReportResultViewModel
    {
        public CrashGameRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<CrashGameRecordViewModel> Records { get; set; }

    }
}
