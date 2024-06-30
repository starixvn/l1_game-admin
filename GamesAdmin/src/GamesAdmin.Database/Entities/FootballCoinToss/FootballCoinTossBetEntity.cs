using GamesAdmin.Database.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("footballcointoss_bets")]
    public class FootballCoinTossBetEntity : BaseBetEntity
    {
        public decimal Home { get; set; }

        public decimal Away { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
        
        public double TotalAmount { get; set; }
    }
}