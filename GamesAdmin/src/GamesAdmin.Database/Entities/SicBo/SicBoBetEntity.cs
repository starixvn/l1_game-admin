using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("sicbo_cuocs")]
    public class SicBoBetEntity : MiniGameBaseEntity
    {
        [BsonElement("phien")]
        public long Round { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        public int? MemberId { get; set; }

        public string SiteId { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}