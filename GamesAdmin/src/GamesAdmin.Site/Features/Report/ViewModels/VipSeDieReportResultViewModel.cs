using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class VipSeDieReportResultViewModel
    {
        public SeDieRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<VipSeDieRecordViewModel> Records { get; set; }
    }
}