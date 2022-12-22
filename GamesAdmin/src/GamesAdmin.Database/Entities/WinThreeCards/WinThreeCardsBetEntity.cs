using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("winthreecards_bets")]
    public class WinThreeCardsBetEntity : BaseBetEntity
    {
        public Dictionary<string, decimal> Odds { get; set; }

        public decimal BetWin { get; set; }

        public decimal Dragon { get; set; }

        public decimal Phoenix { get; set; }

        public decimal Pair8Plus { get; set; }

        public decimal Straight { get; set; }

        public decimal Flush { get; set; }

        public decimal StraightFlush { get; set; }

        [BsonElement("threeOfAKind")]
        public decimal ThreeKind { get; set; }
    }
}