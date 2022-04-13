using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CascadingSlotReportResultViewModel
    {
        public bool ConvertToUSD { get; set; }

        public IEnumerable<CascadingSlotRecordViewModel> Records { get; set; }
    }
}