using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("taixiu_cuocs")]
    public class BigSmallBetEntity : BaseBetEntity
    {
        [BsonElement("taixiu")]
        public bool BigSmall { get; set; }
    }
}