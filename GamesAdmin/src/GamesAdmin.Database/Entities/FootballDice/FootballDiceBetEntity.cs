using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("footballdice_bets")]
    public class FootballDiceBetEntity : BaseBetEntity
    {
        public decimal Home { get; set; }

        public decimal Away { get; set; }

        public decimal Draw { get; set; }

        public double TotalAmount { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
    }
}