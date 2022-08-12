using GamesAdmin.Site._Shared.Configurations;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class VipSeDieWarReportResultViewModel: ReportResultViewModel
    {

        public VipSeDieWarRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<VipSeDieWarRecordViewModel> Records { get; set; }
    }
}