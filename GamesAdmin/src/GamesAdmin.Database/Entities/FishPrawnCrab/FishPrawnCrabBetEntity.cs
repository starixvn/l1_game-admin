using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("baucua_cuocs")]
    public class FishPrawnCrabBetEntity : BaseBetEntity
    {
        [BsonElement("0")]
        public int Stag { get; set; }

        [BsonElement("1")]
        public int Gourd { get; set; }

        [BsonElement("2")]
        public int Rooster { get; set; }

        [BsonElement("3")]
        public int Fish { get; set; }

        [BsonElement("4")]
        public int Crab { get; set; }

        [BsonElement("5")]
        public int Prawn { get; set; }
    }
}