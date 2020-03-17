using MongoDbGenericRepository.Attributes;
using MongoDbGenericRepository.Models;

namespace GamesAdmin.Database.Entities
{
    [CollectionName("game_configs")]
    public class GameConfigEntity: Document
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string id { get; set; }

        public string name { get; set; }

        public bool enabled { get; set; }

        public bool bot { get; set; }

        public double minbet { get; set; }

        public double maxbet { get; set; }
    }
}
