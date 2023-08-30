using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class ColorGameReportResultViewModel : ReportResultViewModel
    {
        public ColorGameRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<ColorGameRecordViewModel> Records { get; set; }
    }
}