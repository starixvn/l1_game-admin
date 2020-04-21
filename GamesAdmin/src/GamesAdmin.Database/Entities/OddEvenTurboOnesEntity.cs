using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("chanleturbo_ones")]
    public class OddEvenTurboOnesEntity : MiniGameBaseEntity
    {
        [BsonElement("tralai")]
        public decimal Back { get; set; }

        [BsonElement("thuong")]
        public decimal Bonus { get; set; }

        public bool Win { get; set; }

        public decimal Betwin { get; set; }

        [BsonElement("thanhtoan")]
        public bool Paid { get; set; }

        public string Uid { get; set; }

        [BsonElement("phien")]
        public long Round { get; set; }

        public bool Select { get; set; }

        public bool Red { get; set; }

        public decimal Bet { get; set; }

        public int? MemberId { get; set; }
    }
}