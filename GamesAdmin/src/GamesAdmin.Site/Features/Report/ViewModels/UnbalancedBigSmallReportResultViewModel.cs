using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class UnbalancedBigSmallReportResultViewModel : ReportResultViewModel
    {
        public int Game { get; set; }

        public UnbalancedBigSmallRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<UnbalancedBigSmallRecordViewModel> Records { get; set; }
    }
}