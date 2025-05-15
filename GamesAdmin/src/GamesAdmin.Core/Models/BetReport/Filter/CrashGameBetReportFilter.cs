namespace GamesAdmin.Core.Models.BetReport
{
    public class CrashGameBetReportFilter : BetReportFilter
    {
        public GeneralGameSetting GameInfo { get; set; }
        public new string Game { get; set; }
    }
}
