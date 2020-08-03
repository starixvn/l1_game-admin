namespace GamesAdmin.Core.Models.DailySummary
{
    public class DailySummaryResult
    {
        public DailySummaryResult(
            string currency,
            decimal stake,
            decimal payout,
            int ticketCount)
        {
            Currency = currency;
            Stake = stake;
            Payout = payout;
            TicketCount = ticketCount;
        }

        public string Currency { get; set; }

        public decimal Stake { get; set; }

        public decimal Payout { get; set; }

        public int TicketCount { get; set; }
    }
}