using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class ShakeThePlateReportResultViewModel
    {
        public ShakeThePlateRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<ShakeThePlateRecordViewModel> Records { get; set; }
    }
}