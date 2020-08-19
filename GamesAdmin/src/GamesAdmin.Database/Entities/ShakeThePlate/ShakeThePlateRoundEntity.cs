using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("xocxoc_phiens")]
    [BsonIgnoreExtraElements]
    public class SeDieRoundEntity : MiniGameBaseEntity
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

        public  SeDieResultEntity Result {get;set;}

        public SeDieSettlementResultEntity SettlementResult { get; set; }
    }

    public class SeDieResultEntity
    {
        [BsonElement("red1")]
        public bool FirstRedChip { get; set; }

        [BsonElement("red2")]
        public bool SecondRedChip { get; set; }

        [BsonElement("red3")]
        public bool ThirdRedChip { get; set; }

        [BsonElement("red4")]
        public bool FourthRedChip { get; set; }
    }

    public class SeDieSettlementResultEntity
    {
        public bool Odd { get; set; }

        public bool Even { get; set; }

        [BsonElement("red3")]
        public bool ThreeRed { get; set; }

        [BsonElement("red4")]
        public bool FourRed { get; set; }

        [BsonElement("white3")]
        public bool ThreeWhite { get; set; }

        [BsonElement("white4")]
        public bool FourWhite { get; set; }
    }
}