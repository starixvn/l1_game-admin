using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("lottery_ones")]
    public class LotteryOnesEntity : BaseBetEntity
    {
        public LotteryChoices Choices { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }

        public decimal TotalAmount { get; set; }
    }

    public class LotteryChoices
    {
        [BsonElement("4Sets")]
        public Dictionary<string, decimal> FourSets { get; set; }

        public Dictionary<string, decimal> Special { get; set; }

        public Dictionary<string, decimal> Head { get; set; }

        public Dictionary<string, decimal> Tail { get; set; }

        public Dictionary<string, decimal> Parlay2 { get; set; }

        public Dictionary<string, decimal> Parlay3 { get; set; }

        public Dictionary<string, decimal> Parlay4 { get; set; }

        [BsonElement("27Sets")]
        public Dictionary<string, decimal> TwentySevenSets { get; set; }

        public Dictionary<string, decimal> Others { get; set; }
    }
}