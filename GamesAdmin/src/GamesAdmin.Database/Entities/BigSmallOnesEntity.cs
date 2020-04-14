using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("taixiu_ones")]
    public class BigSmallOnesEntity : MiniGameBaseEntity
    {
        public bool red { get; set; }

        public decimal bet { get; set; }

        [BsonElement("phien")]
        public long round { get; set; }

        public bool select { get; set; }

        public decimal betwin { get; set; }

        public bool win { get; set; }

        [BsonElement("tralai")]
        public decimal back { get; set; }

        [BsonElement("thuong")]
        public decimal bonus { get; set; }

        public string uid { get; set; }

        [BsonElement("taixiu")]
        public bool sicbo { get; set; }
    }
}
