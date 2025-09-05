using GamesAdmin.Core.Enumeration;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoMatchReportResultViewModel : ReportResultViewModel
    {
        public int Game { get; set; }

        public KenoMatchRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<KenoMatchRecordViewModel> Records { get; set; }
    }
}