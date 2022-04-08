namespace GamesAdmin.Database.Report.Entities
{
    public class DragonTigerBetReportEntity : BetReportEntity
    {
        public decimal BetWin { get; set; }

        public decimal Dragon { get; set; }

        public decimal Tie { get; set; }

        public decimal Tiger { get; set; }
    }
}