using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class SlotRNGReportResultViewModel : ReportResultViewModel
    {
        public SlotRNGRoundResultViewModel RoundResult { get; set; }
        public IEnumerable<SlotRNGRecordViewModel> Records { get; set; }
    }
}