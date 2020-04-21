using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BigSmallTurboReportResultViewModel
    {
        public RoundResultViewModel RoundResult { get; set; }

        public IEnumerable<RecordViewModel> Records { get; set; }
    }
}