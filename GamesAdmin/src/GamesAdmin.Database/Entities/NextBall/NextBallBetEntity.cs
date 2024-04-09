using GamesAdmin.Database.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("luckynext1_bets")]
    public class LuckyNext1BetEntity : BaseBetEntity
    {
        public decimal Over { get; set; }

        public decimal Under { get; set; }

        public decimal Odd { get; set; }

        public decimal Even { get; set; }

        public double TotalAmount { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
    }

    [BsonCollection("luckynext2_bets")]
    public class LuckyNext2BetEntity : LuckyNext1BetEntity
    {
    }
}