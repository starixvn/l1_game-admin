using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Api.Games.Entities
{
    public class GameEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }
    }
}
