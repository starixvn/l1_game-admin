using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("xocxoc_cuocs")]
    public class ShakeThePlateBetEntity : MiniGameBaseEntity
    {
        [BsonElement("le")]
        public int Odd { get; set; }

        [BsonElement("chan")]
        public int Even { get; set; }

        [BsonElement("red3")]
        public int ThreeRed { get; set; }

        [BsonElement("red4")]
        public int FourRed { get; set; }

        [BsonElement("white3")]
        public int ThreeWhite { get; set; }

        [BsonElement("white4")]
        public int FourWhite { get; set; }

        [BsonElement("thanhtoan")]
        public bool Paid { get; set; }

        public decimal Betwin { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        [BsonElement("phien")]
        public long Round { get; set; }

        public bool Red { get; set; }

        public int? MemberId { get; set; }
    }
}