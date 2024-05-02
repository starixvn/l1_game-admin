using GamesAdmin.Database.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("cointoss_bets")]
    public class CoinTossBetEntity : BaseBetEntity
    {
        public decimal Head { get; set; }

        public decimal Tail { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
        
        public double TotalAmount { get; set; }
    }
}