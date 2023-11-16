namespace GamesAdmin.Core.Models.BetReport
{
    public class CrashGameBetRecord: BetRecord
    {
        public long RoundId { get; set; }

        public decimal Amount { get; set; }

        public int ExchangeRateId { get; set; }

        public CrashGameBetHistory HalfCashOutBetLog { get; set; }

        public CrashGameBetHistory CashOutBetLog { get; set; }

    }
}
