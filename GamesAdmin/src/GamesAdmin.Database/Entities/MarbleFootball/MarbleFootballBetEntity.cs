using GamesAdmin.Database.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("marblefootball_bets")]
    public class MarbleFootballBetEntity : BaseBetEntity
    {
        public decimal Home { get; set; }

        public decimal Away { get; set; }

        public decimal Draw { get; set; }

        public decimal Over { get; set; }

        public decimal Under { get; set; }

        public decimal Odd { get; set; }

        public decimal Even { get; set; }

        public double TotalAmount { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
    }
}