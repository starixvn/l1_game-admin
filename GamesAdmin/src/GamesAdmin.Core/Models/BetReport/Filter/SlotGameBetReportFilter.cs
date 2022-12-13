using System;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Core.Models.BetReport
{
    public class SlotGameBetReportFilter : BetReportFilter
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}