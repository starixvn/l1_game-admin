using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("taixiu_ones")]
    public class BigSmallOnesEntity : MiniGameBaseEntity
    {
        public bool Red { get; set; }

        public decimal Bet { get; set; }

        [BsonElement("phien")]
        public long Round { get; set; }

        public bool Select { get; set; }

        public decimal Betwin { get; set; }

        public bool Win { get; set; }

        [BsonElement("tralai")]
        public decimal Back { get; set; }

        [BsonElement("thuong")]
        public decimal Bonus { get; set; }

        public string Uid { get; set; }

        [BsonElement("taixiu")]
        public bool BigSmall { get; set; }

        public int? MemberId { get; set; }

        [BsonElement("thanhtoan")]
        public bool Paid { get; set; }
    }
}
