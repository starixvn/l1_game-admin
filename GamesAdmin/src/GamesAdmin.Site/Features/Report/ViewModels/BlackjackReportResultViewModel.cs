using GamesAdmin.Site._Shared.Configurations;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BlackjackReportResultViewModel : ReportResultViewModel
    {
        public IEnumerable<BlackjackRecordViewModel> Records { get; set; }
    }
}