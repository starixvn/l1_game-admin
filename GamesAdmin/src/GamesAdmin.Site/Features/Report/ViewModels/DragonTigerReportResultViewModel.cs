using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class DragonTigerReportResultViewModel : ReportResultViewModel
    {
        public DragonTigerRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<DragonTigerRecordViewModel> Records { get; set; }
    }
}