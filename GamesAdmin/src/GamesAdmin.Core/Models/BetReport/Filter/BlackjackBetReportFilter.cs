using System;

namespace GamesAdmin.Core.Models.BetReport
{
    public class BlackjackBetReportFilter : BetReportFilter
    {
        public long HandId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}