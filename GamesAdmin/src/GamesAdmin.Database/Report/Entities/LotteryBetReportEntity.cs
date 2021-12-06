namespace GamesAdmin.Database.Report.Entities
{
    public class LotteryBetReportEntity : BetReportEntity
    {
        public decimal BetWin { get; set; }

        public decimal TotalBet { get; set; }
    }
}