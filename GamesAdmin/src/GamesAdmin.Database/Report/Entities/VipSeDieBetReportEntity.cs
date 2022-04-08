namespace GamesAdmin.Database.Report.Entities
{
    public class VipSeDieBetReportEntity : BetReportEntity
    {
        public decimal Odd { get; set; }

        public decimal Even { get; set; }

        public decimal TwoRed { get; set; }

        public decimal ThreeRed { get; set; }

        public decimal FourRed { get; set; }

        public decimal ThreeWhite { get; set; }

        public decimal FourWhite { get; set; }

        public decimal BetWin { get; set; }

        public string UserId { get; set; }
    }
}