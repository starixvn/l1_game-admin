using GamesAdmin.Database.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("sedie3_bets")]
    public class SeDie3BetEntity : BaseBetEntity
    {
        public decimal Red { get; set; }

        public decimal White { get; set; }

        [BsonElement("red1")]
        public decimal OneRed { get; set; }

        [BsonElement("red2")]
        public decimal TwoRed { get; set; }

        [BsonElement("red3")]
        public decimal ThreeRed { get; set; }

        [BsonElement("white3")]
        public decimal ThreeWhite { get; set; }
    }
}
