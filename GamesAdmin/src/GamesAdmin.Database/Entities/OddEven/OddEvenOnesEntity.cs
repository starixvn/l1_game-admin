using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("chanle_ones")]
    public class OddEvenOnesEntity : BaseBetEntity
    {
        [BsonElement("tralai")]
        public decimal Back { get; set; }

        [BsonElement("thuong")]
        public decimal Bonus { get; set; }

        public bool Win { get; set; }

        public bool Select { get; set; }

        public decimal Bet { get; set; }
    }
}