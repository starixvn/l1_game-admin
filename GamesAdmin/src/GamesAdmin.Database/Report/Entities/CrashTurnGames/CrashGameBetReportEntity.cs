using System.Collections.Generic;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Database.Report.Entities
{
    public class CrashTurnGameBetReportEntity : BetReportEntity
    {
        public long Round { get; set; }

        public decimal BetWin { get; set; }

        public decimal Amount { get; set; }
        public decimal Result { get; set; }

        public int ExchangeRateId { get; set; }

        public IEnumerable<CrashTurnGameBetLogEntity> BetLogs { get; set; }
    }
}