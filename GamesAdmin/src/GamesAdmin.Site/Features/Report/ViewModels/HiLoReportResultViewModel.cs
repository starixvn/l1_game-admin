using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class HiLoReportResultViewModel : ReportResultViewModel
    {
        public IEnumerable<HiLoRecordViewModel> Records { get; set; }
    }
}