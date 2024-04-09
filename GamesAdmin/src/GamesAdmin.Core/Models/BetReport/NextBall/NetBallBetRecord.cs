namespace GamesAdmin.Core.Models
{
    public class NextBallBetRecord : BetRecord
    {
        public decimal Over { get; set; }

        public decimal Under { get; set; }

        public decimal Odd { get; set; }

        public decimal Even { get; set; }

        public double TotalAmount { get; set; }

        public long DrawId { get; set; }
    }
}