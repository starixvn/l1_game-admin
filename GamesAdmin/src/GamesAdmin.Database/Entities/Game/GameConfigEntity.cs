using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities.Game
{
    public class GameConfigEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        public string name { get; set; }

        public bool enabled { get; set; }

        public bool bot { get; set; }

        public double minbet { get; set; }

        public double maxbet { get; set; }
    }
}
