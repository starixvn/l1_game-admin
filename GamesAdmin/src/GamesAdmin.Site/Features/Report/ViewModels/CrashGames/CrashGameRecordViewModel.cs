using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CrashGameRecordViewModel : BetRecordViewModel
    {
        public string Game { get; set; }

        public long RoundId { get; set; }

        private decimal baseAmount { get; set; }

        public decimal Amount
        {
            get => baseAmount * CurrencyMultiplier;
            set
            {
                baseAmount = value;
            }
        }

        private decimal baseBetWin { get; set; }

        public new decimal BetWin
        {
            get => baseBetWin * CurrencyMultiplier;
            set
            {
                baseBetWin = value;
            }
        }

        public int ExchangeRateId { get; set; }

        public decimal CurrencyMultiplier { get; set; }

        public IEnumerable<CrashGameBetHistoryViewModel> BetLogs { get; set; }
        public bool HasHalfCashOut => BetLogs.Any(x => x.Action == CrashGameAction.HalfCashOut);
        public bool HasCashOut => BetLogs.Any(x => x.Action == CrashGameAction.CashOut);
        public bool HasRefund => BetLogs.Any(x => x.Action == CrashGameAction.Refund);

        public CrashGameBetHistoryViewModel CashOutBetLog => BetLogs.FirstOrDefault(x => x.Action.Equals(CrashGameAction.CashOut));

        public CrashGameBetHistoryViewModel HalfCashOutBetLog => BetLogs.FirstOrDefault(x => x.Action.Equals(CrashGameAction.HalfCashOut));

        public CrashGameBetHistoryViewModel RefundBetLog => BetLogs.FirstOrDefault(x => x.Action.Equals(CrashGameAction.Refund));
    }
}
