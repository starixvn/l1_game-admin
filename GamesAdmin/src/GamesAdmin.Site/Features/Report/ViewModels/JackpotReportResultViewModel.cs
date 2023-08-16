using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class JackpotReportResultViewModel
    {
        public bool EnableConvertBase { get; set; }

        public IEnumerable<JackpotRecordViewModel> Records { get; set; }
    }
}
