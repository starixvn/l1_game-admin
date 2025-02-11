using System.Collections.Generic;

namespace GamesAdmin.Site.Features.DailySummary.Models
{
    public class DailySummaryResult
    {
        public IEnumerable<SummaryItem> Summary { get; set; }
    }

    public class SummaryItem
    {
        public string Currency { get; set; }

        public decimal Stake { get; set; }

        public decimal Payout { get; set; }

        public int TicketCount { get; set; }
    }
}
