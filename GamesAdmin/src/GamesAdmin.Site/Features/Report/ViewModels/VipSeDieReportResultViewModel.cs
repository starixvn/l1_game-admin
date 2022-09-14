using GamesAdmin.Site._Shared.Configurations;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class VipSeDieReportResultViewModel : ReportResultViewModel
    {
        public SeDieRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<VipSeDieRecordViewModel> Records { get; set; }
    }
}