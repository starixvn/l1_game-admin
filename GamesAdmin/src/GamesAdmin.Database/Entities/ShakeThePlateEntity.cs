using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("xocxoc_cuocs")]
    public class ShakeThePlateBetEntity : MiniGameBaseEntity
    {
        [BsonElement("chan")]
        public decimal Even { get; set; }

        [BsonElement("le")]
        public decimal Odd { get; set; }

        [BsonElement("red3")]
        public decimal ThreeRed { get; set; }

        [BsonElement("red4")]
        public decimal FourRed { get; set; }

        [BsonElement("white3")]
        public decimal ThreeWhite { get; set; }

        [BsonElement("white4")]
        public decimal FourWhite { get; set; }

        [BsonElement("thanhtoan")]
        public bool Paid { get; set; }

        public decimal Betwin { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        [BsonElement("phien")]
        public long Round { get; set; }

        public bool Red { get; set; }
    }

    [BsonCollection("xocxoc_phiens")]
    [BsonIgnoreExtraElements]
    public class ShakeThePlateRoundEntity : MiniGameBaseEntity
    {
        [BsonElement("id")]
        public long Number { get; set; }

        [BsonElement("red1")]
        public bool FirstRedChip { get; set; }

        [BsonElement("red2")]
        public bool SecondRedChip { get; set; }

        [BsonElement("red3")]
        public bool ThirdRedChip { get; set; }

        [BsonElement("red4")]
        public bool FourthRedChip { get; set; }
    }
}