using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoMatchReportResultViewModel
    {
        public GameId Game { get; set; }

        public KenoMatchRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<KenoMatchRecordViewModel> Records { get; set; }
    }
}