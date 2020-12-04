using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("vipbaccarat_phiens")]
    public class VipBaccaratRoundEntity : BaseRoundEntity
    {
        public VipBaccaratResultEntity Result { get; set; }

        public BaccaratSettlementResultEntity SettlementResult { get; set; }
    }

    public class VipBaccaratResultEntity
    {
        public Side Player { get; set; }

        public Side Banker { get; set; }
    }
}
