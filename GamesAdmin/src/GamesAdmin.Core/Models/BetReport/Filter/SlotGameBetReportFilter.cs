using System;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Core.Models.BetReport
{
    public class SlotGameBetReportFilter : BetReportFilter
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Username { get; set; }
        public bool ConvertToUSD { get; set; }
        public Game Game { get; set; }
    }
}