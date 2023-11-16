namespace GamesAdmin.Core.Models.BetReport
{
    public class SpeedUpGameBetReportFilter : BetReportFilter
    {
        public GeneralGameSetting GameInfo { get; set; }
        public new string Game { get; set; }
    }
}
