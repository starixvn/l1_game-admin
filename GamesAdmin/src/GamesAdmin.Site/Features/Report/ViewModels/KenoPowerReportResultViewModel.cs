using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoPowerReportResultViewModel : ReportResultViewModel
    {
        public GameId Game { get; set; }

        public KenoPowerRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<KenoPowerRecordViewModel> Records { get; set; }
    }
}