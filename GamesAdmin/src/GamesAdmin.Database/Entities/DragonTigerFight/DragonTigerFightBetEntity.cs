using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("dragontigerfight_bets")]
    public class DragonTigerFightBetEntity : DragonTigerBetEntity
    {
        public Dictionary<string, decimal> Odds { get; set; }
        public decimal BlackDragon { get; set; }
        public decimal RedDragon { get; set; }
        public decimal BlackTiger { get; set; }
        public decimal RedTiger { get; set; }
        public double TotalAmount { get; set; }
    }
}