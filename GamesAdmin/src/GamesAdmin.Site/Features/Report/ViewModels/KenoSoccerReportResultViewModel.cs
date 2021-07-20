using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoSoccerReportResultViewModel
    {
        public GameId Game { get; set; }

        public KenoSoccerRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<KenoSoccerRecordViewModel> Records { get; set; }
    }
}