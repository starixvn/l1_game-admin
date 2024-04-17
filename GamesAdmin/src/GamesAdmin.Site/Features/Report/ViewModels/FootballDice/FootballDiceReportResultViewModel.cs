using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class FootballDiceReportResultViewModel : ReportResultViewModel
    {
        public FootballDiceRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<FootballDiceRecordViewModel> Records { get; set; }
    }
}