using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class OddEvenReportResultViewModel
    {
        public string ConvertCurrency { get; set; }

        public RoundResultViewModel RoundResult { get; set; }

        public IEnumerable<OddEvenRecordViewModel> Records { get; set; }
    }
}