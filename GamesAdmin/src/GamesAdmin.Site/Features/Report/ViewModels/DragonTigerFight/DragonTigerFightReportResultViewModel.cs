using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class DragonTigerFightReportResultViewModel : ReportResultViewModel
    {
        public DragonTigerFightRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<DragonTigerFightRecordViewModel> Records { get; set; }
    }
}