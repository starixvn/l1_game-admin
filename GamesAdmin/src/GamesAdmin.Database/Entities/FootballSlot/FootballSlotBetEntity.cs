using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("footballslot_bets")]
    public class FootballSlotBetEntity : BaseBetEntity
    {
        public Dictionary<string, decimal> Odds { get; set; }
        public double TotalAmount { get; set; }
    }
}