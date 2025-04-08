using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class CrashGameBetRecord: BetRecord
    {
        public long RoundId { get; set; }

        public decimal Amount { get; set; }

        public int ExchangeRateId { get; set; }
        
        public string ClientSeed { get; set; }

        public IEnumerable<CrashGameBetHistory> BetLogs { get; set; }
    }
}
