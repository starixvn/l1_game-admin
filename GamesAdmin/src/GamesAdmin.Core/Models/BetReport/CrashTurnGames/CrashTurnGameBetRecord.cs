using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class CrashTurnGameBetRecord: BetRecord
    {
        public long RoundId { get; set; }

        public decimal Amount { get; set; }
        public decimal Result { get; set; }

        public IEnumerable<CrashTurnGameBetHistory> BetLogs { get; set; }
    }
}
