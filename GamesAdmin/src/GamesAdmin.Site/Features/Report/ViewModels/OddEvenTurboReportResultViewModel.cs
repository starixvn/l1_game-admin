using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class OddEvenTurboReportResultViewModel
    {
        public string ConvertCurrency { get; set; }

        public RoundResultViewModel RoundResult { get; set; }

        public IEnumerable<OddEvenRecordViewModel> Records { get; set; }
    }
}