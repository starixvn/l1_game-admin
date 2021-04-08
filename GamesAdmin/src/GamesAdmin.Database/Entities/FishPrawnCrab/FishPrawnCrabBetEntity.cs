using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("baucua_cuocs")]
    public class FishPrawnCrabBetEntity : BaseBetEntity
    {
        [BsonElement("0")]
        public decimal? OldStag { get; set; }

        [BsonElement("1")]
        public decimal? OldGourd { get; set; }

        [BsonElement("2")]
        public decimal? OldRooster { get; set; }

        [BsonElement("3")]
        public decimal? OldFish { get; set; }

        [BsonElement("4")]
        public decimal? OldCrab { get; set; }

        [BsonElement("5")]
        public decimal? OldPrawn { get; set; }

        public decimal Stag { get; set; }

        public decimal Gourd { get; set; }

        public decimal Rooster { get; set; }

        public decimal Fish { get; set; }

        public decimal Crab { get; set; }

        public decimal Prawn { get; set; }
    }
}