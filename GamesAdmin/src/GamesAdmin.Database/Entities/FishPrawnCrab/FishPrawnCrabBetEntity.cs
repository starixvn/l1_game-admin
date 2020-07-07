using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("baucua_cuocs")]
    public class FishPrawnCrabBetEntity : BaseBetEntity
    {
        [BsonElement("0")]
        public decimal Stag { get; set; }

        [BsonElement("1")]
        public decimal Gourd { get; set; }

        [BsonElement("2")]
        public decimal Rooster { get; set; }

        [BsonElement("3")]
        public decimal Fish { get; set; }

        [BsonElement("4")]
        public decimal Crab { get; set; }

        [BsonElement("5")]
        public decimal Prawn { get; set; }
    }
}