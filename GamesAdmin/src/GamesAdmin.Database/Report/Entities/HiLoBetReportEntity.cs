namespace GamesAdmin.Database.Report.Entities
{
    public class HiLoBetReportEntity : BetReportEntity
    {
        public decimal BetWin { get; set; }

        public decimal Amount { get; set; }

        public long RoundId { get; set; }

        public bool Paid { get; set; }

        public int ExchangeRateId { get; set; }
    }
}