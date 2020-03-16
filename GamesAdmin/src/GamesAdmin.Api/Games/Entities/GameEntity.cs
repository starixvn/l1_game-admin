using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Api.Games.Entities
{
    public class GameEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string name { get; set; }

        public bool enabled { get; set; }

        public bool bot { get; set; }
    }
}
