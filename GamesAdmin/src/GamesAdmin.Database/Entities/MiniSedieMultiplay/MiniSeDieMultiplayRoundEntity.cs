using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("minisedie2_rounds")]
    [BsonIgnoreExtraElements]
    public class MiniSeDie2RoundEntity : MiniSeDieMultiplayRoundEntity
    {
    }

    [BsonCollection("minisedie3_rounds")]
    [BsonIgnoreExtraElements]
    public class MiniSeDie3RoundEntity : MiniSeDieMultiplayRoundEntity
    {
    }

    [BsonCollection("minisedie4_rounds")]
    [BsonIgnoreExtraElements]
    public class MiniSeDie4RoundEntity : MiniSeDieMultiplayRoundEntity
    {
    }

    public class MiniSeDieMultiplayRoundEntity : BaseRoundEntity
    {
        [BsonElement("red1")]
        public bool? FirstRedChip { get; set; }

        [BsonElement("red2")]
        public bool? SecondRedChip { get; set; }

        [BsonElement("red3")]
        public bool? ThirdRedChip { get; set; }

        [BsonElement("red4")]
        public bool? FourthRedChip { get; set; }

        public MiniSedieMultiplayResult Result { get; set; }

        public MiniSeDieMultiplaySettlementResultEntity SettlementResult { get; set; }
    }

    public class MiniSedieMultiplayResult
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

    public class MiniSeDieMultiplaySettlementResultEntity
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