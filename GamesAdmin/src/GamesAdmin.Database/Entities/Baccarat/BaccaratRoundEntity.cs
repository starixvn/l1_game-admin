using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("baccarat_phiens")]
    public class BaccaratRoundEntity : BaseBaccaratRoundEntity
    {
        public Side Player { get; set; }

        public Side Banker { get; set; }

        public BaccaratSettlementResultEntity Result { get; set; }
    }

    public class BaseBaccaratRoundEntity : BaseRoundEntity
    {
    }

    public class BaccaratSettlementResultEntity
    {
        public bool Player { get; set; }
        public bool Banker { get; set; }
        public bool Tie { get; set; }
        public bool Big { get; set; }
        public bool Small { get; set; }
        public bool PlayerPair { get; set; }
        public bool BankerPair { get; set; }
        public bool Natural { get; set; }
    }

    public class Side
    {
        [BsonElement("card1")]
        public Card FirstCard { get; set; }

        [BsonElement("card2")]
        public Card SecondCard { get; set; }

        [BsonElement("card3")]
        public Card ThirdCard { get; set; }
    }
}