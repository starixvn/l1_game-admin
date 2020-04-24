using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("txturbo_cuocs")]
    public class BigSmallTurboBetEntity : BaseBetEntity
    {
        [BsonElement("taixiu")]
        public bool BigSmall { get; set; }
    }
}