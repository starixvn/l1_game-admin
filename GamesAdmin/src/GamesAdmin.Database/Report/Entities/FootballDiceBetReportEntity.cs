namespace GamesAdmin.Database.Report.Entities
{
    public class FootballDiceBetReportEntity : BetReportEntity
    {
        public decimal Home { get;set; }
        public decimal Away { get;set; }
        public decimal Draw { get;set; }

        public bool FreeBet { get;set; }
        public decimal BetWin { get; set; }

        public decimal TotalBet { get; set; }
    }
}