using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class VipSeDieWarReportResultViewModel
    {
        public VipSeDieWarRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<VipSeDieWarRecordViewModel> Records { get; set; }
    }
}