using System;

namespace GamesAdmin.Core.Models.BetReport
{
    public class SlotGameBetReportFilter : BetReportFilter
    {
        public GeneralGameSetting GameInfo { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public new string Game { get; set; }
    }
}