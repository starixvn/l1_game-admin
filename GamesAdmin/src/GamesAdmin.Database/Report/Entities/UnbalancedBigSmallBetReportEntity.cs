namespace GamesAdmin.Database.Report.Entities
{
    public class UnbalancedBigSmallBetReportEntity : BetReportEntity
    {
        public decimal Big { get; set; }

        public decimal Small { get; set; }

        public decimal BetWin { get; set; }

        public decimal TotalBet { get; internal set; }
    }
}