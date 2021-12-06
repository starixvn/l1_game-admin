using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoRecordViewModel: BetRecordViewModel
    {
        public Dictionary<string, decimal> Amounts { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }

        public decimal Total { get; set; }
    }
}
