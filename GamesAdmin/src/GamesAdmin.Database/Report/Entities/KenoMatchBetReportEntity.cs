namespace GamesAdmin.Database.Report.Entities
{
    public class KenoMatchBetReportEntity : BetReportEntity
    {
        public decimal TotalBet { get; set; }
        public decimal Refund { get; set; }
        public string UserId { get; set; }
    }
}