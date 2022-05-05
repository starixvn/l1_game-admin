namespace GamesAdmin.Database.Report.Entities
{
    public class KenoPowerBetReportEntity : BetReportEntity
    {
        public decimal BetWin { get; set; }

        public decimal TotalBet { get; set; }
        public decimal Refund { get; set; }

        public string UserId { get; set; }
    }
}