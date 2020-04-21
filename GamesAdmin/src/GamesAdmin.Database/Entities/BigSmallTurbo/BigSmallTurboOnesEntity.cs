using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("txturbo_ones")]
    public class BigSmallTurboOnesEntity : MiniGameBaseEntity
    {
        [BsonElement("tralai")]
        public decimal Back { get; set; }

        [BsonElement("thuong")]
        public decimal Bonus { get; set; }

        public bool Win { get; set; }

        public decimal Betwin { get; set; }

        public string Uid { get; set; }

        [BsonElement("phien")]
        public long Round { get; set; }

        [BsonElement("taixiu")]
        public bool BigSmall { get; set; }

        public bool Select { get; set; }

        public bool Red { get; set; }

        public decimal Bet { get; set; }

        public int? MemberId { get; set; }

        [BsonElement("thanhtoan")]
        public bool Paid { get; set; }
    }
}