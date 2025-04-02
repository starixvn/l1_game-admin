using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("marblefootball2_bets")]
    public class MarbleFootball2BetEntity : BaseBetEntity
    {
        public decimal Home { get; set; }

        public decimal Away { get; set; }

        public decimal Draw { get; set; }

        public decimal Over { get; set; }

        public decimal Under { get; set; }

        public decimal Odd { get; set; }

        public decimal Even { get; set; }

        public decimal HomeLive { get; set; }

        public decimal AwayLive { get; set; }

        public decimal DrawLive { get; set; }

        public decimal OverLive { get; set; }

        public decimal UnderLive { get; set; }

        public decimal OddLive { get; set; }

        public decimal EvenLive { get; set; }

        public double TotalAmount { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
    }
}
