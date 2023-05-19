using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class SlotGameReportResultViewModel
    {
        public bool ConvertToUSD { get; set; }

        public IEnumerable<SlotGameRecordViewModel> Records { get; set; }
    }
}