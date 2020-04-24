using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("baucua_cuocs")]
    public class FishPrawnCrabBetEntity : MiniGameBaseEntity
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

        [BsonElement("thanhtoan")]
        public bool Paid { get; set; }

        public bool BigWin { get; set; }

        public decimal Betwin { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        [BsonElement("phien")]
        public long Round { get; set; }

        public bool Red { get; set; }

        public int? MemberId { get; set; }
    }
}