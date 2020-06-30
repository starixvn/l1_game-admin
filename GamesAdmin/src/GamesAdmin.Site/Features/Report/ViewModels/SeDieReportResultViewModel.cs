using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class SeDieReportResultViewModel
    {
        public SeDieRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<SeDieRecordViewModel> Records { get; set; }
    }
}