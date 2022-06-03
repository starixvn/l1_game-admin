using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("blockchainbaccarat_rounds")]
    public class BlockchainBaccaratRoundEntity : BaseRoundEntity
    {
        public BlockchainBaccaratCardResult Player { get; set; }

        public BlockchainBaccaratCardResult Banker { get; set; }

        public BaccaratSettlementResultEntity Result { get; set; }
    }

    public class BlockchainBaccaratCardResult
    {
        [BsonElement("card1")]
        public BlockchainBaccaratCard FirstCard { get; set; }

        [BsonElement("card2")]
        public BlockchainBaccaratCard SecondCard { get; set; }

        [BsonElement("card3")]
        public BlockchainBaccaratCard ThirdCard { get; set; }
    }

    public class BlockchainBaccaratCard
    {
        [BsonElement("rank")]
        public byte Number { get; set; }

        [BsonElement("suit")]
        public byte Type { get; set; }
    }
}
