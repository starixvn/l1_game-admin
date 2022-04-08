namespace GamesAdmin.Database.Report.Entities
{
    public class StakeBalancedBetReportEntity : BetReportEntity
    {
        public bool Select { get; set; }

        public decimal Bet { get; set; }

        public decimal BetWin { get; set; }

        public decimal Back { get; set; }

        public string Choice { get; set; }

        public bool Win { get; set; }
    }
}
