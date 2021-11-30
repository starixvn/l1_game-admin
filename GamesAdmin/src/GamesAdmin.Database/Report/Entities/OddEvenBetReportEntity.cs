namespace GamesAdmin.Database.Report.Entities
{
    public class OddEvenBetReportEntity : BetReportEntity
    {
        public bool Select { get; set; }

        public decimal Bet { get; set; }

        public decimal BetWin { get; set; }

        public decimal Back { get; set; }
    }
}