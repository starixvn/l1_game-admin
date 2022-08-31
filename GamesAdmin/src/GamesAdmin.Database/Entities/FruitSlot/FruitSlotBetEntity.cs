using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("fruitslot_bets")]
    public class FruitSlotBetEntity : BaseBetEntity
    {
        public Dictionary<string, decimal> Odds { get; set; }
        public double TotalAmount { get; set; }
    }
}