using GamesAdmin.Database.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("xocxoc_cuocs")]
    public class SeDieBetEntity : BaseBetEntity
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

        [BsonElement("le")]
        public decimal OldOdd { get; set; }

        [BsonElement("chan")]
        public decimal OldEven { get; set; }

        public bool? FreeBet { get; set; }
    }
}