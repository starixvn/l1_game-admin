using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CrashGameReportResultViewModel : ReportResultViewModel
    {
        public CrashGameRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<CrashGameRecordViewModel> Records { get; set; }

        public bool HasTwoHand => Records.Any(x => x.HasTwoHand);
    }
}
