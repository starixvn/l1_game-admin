using System;

namespace GamesAdmin.Core.Models.BetReport
{
    public class CrashTurnGameBetReportFilter : CrashGameBetReportFilter
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
