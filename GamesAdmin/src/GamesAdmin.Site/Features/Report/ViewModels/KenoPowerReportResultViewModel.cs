using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site._Shared.Configurations;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoPowerReportResultViewModel : ReportResultViewModel
    {
        public int Game { get; set; }

        public KenoPowerRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<KenoPowerRecordViewModel> Records { get; set; }
    }
}