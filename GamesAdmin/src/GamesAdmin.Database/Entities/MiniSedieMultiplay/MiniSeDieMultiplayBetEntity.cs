using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("minisedie2_bets")]
    public class MiniSeDie2BetEntity : MiniSeDieMultiplayBetEntity
    {
    }

    [BsonCollection("minisedie3_bets")]
    public class MiniSeDie3BetEntity : MiniSeDieMultiplayBetEntity
    {
    }

    [BsonCollection("minisedie4_bets")]
    public class MiniSeDie4BetEntity : MiniSeDieMultiplayBetEntity
    {
    }

    public class MiniSeDieMultiplayBetEntity : BaseBetEntity
    {
        public decimal Odd { get; set; }

        public decimal Even { get; set; }

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