using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class SpecialSeDieReportResultViewModel
    {
        public string ConvertCurrency { get; set; }

        public SeDieRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<SpecialSeDieRecordViewModel> Records { get; set; }
    }
}