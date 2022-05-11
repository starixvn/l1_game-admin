using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CascadingSlotBetHistoryViewModel
    {
        public CascadingSlotRecordViewModel Round { get; set; }

        public IEnumerable<CascadingSlotRoundMultiplierViewModel> Multipliers { get; set; }

        public bool ConvertToUSD { get; set; }

        public string Game { get; set; }
    }
}
