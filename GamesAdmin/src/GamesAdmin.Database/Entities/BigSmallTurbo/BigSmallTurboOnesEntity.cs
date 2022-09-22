using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("txturbo_ones")]
    public class BigSmallTurboOnesEntity : BasePoolBetEntity
    {
        [BsonElement("tralai")]
        public decimal? Back { get; set; }

        [BsonElement("thuong")]
        public decimal Bonus { get; set; }

        public bool Win { get; set; }

        [BsonElement("taixiu")]
        public bool BigSmall { get; set; }

        public bool? Select { get; set; }

        public decimal? Bet { get; set; }

        public decimal Amount { get; set; }
        public string Choice { get; set; }
        public decimal Refund { get; set; }
    }
}