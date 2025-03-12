using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CrashTurnGameRecordViewModel : BetRecordViewModel
    {
        public decimal Result { get; set; }

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

        public IEnumerable<CrashTurnGameBetHistoryViewModel> BetLogs { get; set; }
        public bool HasCashOut => BetLogs.Any(x => x.Action == CrashGameAction.CashOut);

        public CrashTurnGameBetHistoryViewModel CashOutBetLog => BetLogs.FirstOrDefault(x => x.Action.Equals(CrashGameAction.CashOut));
    }
}
