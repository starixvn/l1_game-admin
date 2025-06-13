using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class MiniRouletteReportResultViewModel : ReportResultViewModel
    {
        public MiniRouletteRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<MiniRouletteRecordViewModel> Records { get; set; }
    }
}
