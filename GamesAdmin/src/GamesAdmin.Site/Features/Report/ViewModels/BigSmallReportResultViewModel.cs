using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BigSmallReportResultViewModel
    {
        public string ConvertCurrency { get; set; }

        public RoundResultViewModel RoundResult { get; set; }

        public IEnumerable<BigSmallRecordViewModel> Records { get; set; }
    }
}