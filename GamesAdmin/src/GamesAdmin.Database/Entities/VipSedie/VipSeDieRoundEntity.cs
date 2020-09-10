using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("vipsedie_phiens")]
    public class VipSeDieRoundEntity : BaseRoundEntity
    {
        public VipSeDieResultEntity Result { get; set; }

        public VipSeDieSettlementResultEntity SettlementResult { get; set; }
    }

    public class VipSeDieResultEntity
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

    public class VipSeDieSettlementResultEntity
    {
        public bool Odd { get; set; }

        public bool Even { get; set; }

        [BsonElement("red2")]
        public bool TwoRed { get; set; }

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