using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class FruitSlotReportResultViewModel : ReportResultViewModel
    {
        public FruitSlotRoundResultViewModel RoundResult { get; set; }
        public IEnumerable<FruitSlotRecordViewModel> Records { get; set; }
    }
}
