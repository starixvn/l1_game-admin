using GamesAdmin.Core.Models.BetReport;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CrashGameRecordViewModel : BetRecordViewModel
    {
        public string Game { get; set; }

        public long RoundId { get; set; }

        public decimal Amount { get; set; }

        public int ExchangeRateId { get; set; }

        public CrashGameBetHistory HalfCashOutBetLog { get; set; }

        public CrashGameBetHistory CashOutBetLog { get; set; }
    }
}
