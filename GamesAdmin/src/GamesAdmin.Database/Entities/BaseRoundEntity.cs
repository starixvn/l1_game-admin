using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    public abstract class BaseRoundEntity : MiniGameBaseEntity
    {
        [BsonElement("id")]
        public long Number { get; set; }
    }
}