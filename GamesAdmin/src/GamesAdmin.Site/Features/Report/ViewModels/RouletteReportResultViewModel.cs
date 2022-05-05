using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class RouletteReportResultViewModel : ReportResultViewModel
    {
        public RouletteRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<RouletteRecordViewModel> Records { get; set; }
    }
}