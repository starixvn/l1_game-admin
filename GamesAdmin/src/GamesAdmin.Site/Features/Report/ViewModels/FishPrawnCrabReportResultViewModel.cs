using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class FishPrawnCrabReportResultViewModel : ReportResultViewModel
    {
        public FishPrawnCrabRoundResultViewModel RoundResult { get; set; }
        public IEnumerable<FishPrawnCrabRecordViewModel> Records { get; set; }
    }
}