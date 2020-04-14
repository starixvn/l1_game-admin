using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("taixiu_phiens")]
    [BsonIgnoreExtraElements]
    public class BigSmallRoundEntity : MiniGameBaseEntity
    {        
        [BsonElement("id")]
        public long number { get; set; }

        public int dice1 { get; set; }

        public int dice2 { get; set; }

        public int dice3 { get; set; }
    }
}
