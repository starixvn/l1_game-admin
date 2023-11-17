using System.Collections.Generic;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Database.Report.Entities
{
    public class CrashGameBetReportEntity : BetReportEntity
    {
        public long Round { get; set; }

        public decimal BetWin { get; set; }

        public decimal Amount { get; set; }

        public int ExchangeRateId { get; set; }

        public string OperatorId { get; set; }

        public List<CrashGameBetLogEntity> BetLogs { get; set; }
    }
}
