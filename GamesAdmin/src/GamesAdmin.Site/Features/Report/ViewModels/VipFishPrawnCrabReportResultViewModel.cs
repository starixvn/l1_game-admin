using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class VipFishPrawnCrabReportResultViewModel
    {
        public VipFishPrawnCrabRoundResultViewModel RoundResult { get; set; }
        public IEnumerable<VipFishPrawnCrabRecordViewModel> Records { get; set; }
    }
}