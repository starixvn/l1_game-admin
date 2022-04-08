namespace GamesAdmin.Database.Report.Entities
{
    public class VipSeDieWarBetReportEntity : BetReportEntity
    {
        public decimal BetWin { get; set; }

        public decimal TotalBet { get; set; }

        public string UserId { get; set; }
    }
}