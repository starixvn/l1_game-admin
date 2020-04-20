using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    public abstract class BaseBetEntity : MiniGameBaseEntity
    {
        [BsonElement("tralai")]
        public decimal Back { get; set; }

        [BsonElement("thanhtoan")]
        public bool Paid { get; set; }

        public bool Win { get; set; }

        public decimal Betwin { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        [BsonElement("phien")]
        public long Round { get; set; }

        public decimal Bet { get; set; }

        public bool Select { get; set; }

        public bool Red { get; set; }

        public int? MemberId { get; set; }
    }
}