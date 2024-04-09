using GamesAdmin.Database.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("bacball2_bets")]
    public class BacBall2BetEntity : BaseBetEntity
    {
        public decimal Banker { get; set; }

        public decimal Player { get; set; }

        public decimal Tie { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
        
        public double TotalAmount { get; set; }
    }
}