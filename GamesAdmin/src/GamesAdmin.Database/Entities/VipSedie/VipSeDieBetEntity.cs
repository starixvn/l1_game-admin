using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("vipsedie_cuocs")]
    public class VipSeDieBetEntity : BaseBetEntity
    {
        public decimal Odd { get; set; }

        public decimal Even { get; set; }

        [BsonElement("red2")]
        public decimal TwoRed { get; set; }

        [BsonElement("red3")]
        public decimal ThreeRed { get; set; }

        [BsonElement("red4")]
        public decimal FourRed { get; set; }

        [BsonElement("white3")]
        public decimal ThreeWhite { get; set; }

        [BsonElement("white4")]
        public decimal FourWhite { get; set; }
    }
}