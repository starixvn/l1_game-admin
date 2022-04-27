using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("blockchainbaccarat_phiens")]
    public class BlockchainBaccaratRoundEntity : BaseRoundEntity
    {
        public BlockchainResultEntity Result { get; set; }

        public BaccaratSettlementResultEntity SettlementResult { get; set; }
    }

    public class BlockchainResultEntity
    {
        public Side Player { get; set; }

        public Side Banker { get; set; }
    }
}
