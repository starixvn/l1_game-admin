namespace GamesAdmin.Database.Report.Entities
{
    public class KenoMatchBetReportEntity : BetReportEntity
    {
        public decimal BetWin { get; set; }
        public decimal TotalBet { get; set; }
        public decimal Refund { get; set; }
    }
}