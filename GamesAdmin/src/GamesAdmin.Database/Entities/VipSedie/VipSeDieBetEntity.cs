using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities.VipSedie
{
    [BsonCollection("VipSeDie_cuocs")]
    public class VipSeDieBetEntity : MiniGameBaseEntity
    {
        public string Uid { get; set; }

        public string Name { get; set; }

        [BsonElement("phien")]
        public long Round { get; set; }

        public int? MemberId { get; set; }

        public string SiteId { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}