using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("bac81_rounds")]
    public class Bac81RoundEntity : BaccaratXRoundEntity
    {
    }

    public class BaccaratXRoundEntity
    {
        public BlockchainBaccaratResult Result { get; set; }
        public BaccaratXSettlementResultEntity SettlementResult { get; set; }
        public IEnumerable<BaccaratXMultiplierCardEntity> OddsMultipliers { get; set; }
        public AppliedMultipliers AppliedMultipliers { get; set; }

        public decimal ExtraFeeRatio { get; set; }
    }

    public class AppliedMultipliers
    {
        public IEnumerable<BaccaratXMultiplierCardEntity> Player { get; set; } = Enumerable.Empty<BaccaratXMultiplierCardEntity>();
        public IEnumerable<BaccaratXMultiplierCardEntity> Banker { get; set; } = Enumerable.Empty<BaccaratXMultiplierCardEntity>();
    }

    public class BaccaratXSettlementResultEntity
    {
        public bool Player { get; set; }
        public bool Banker { get; set; }
        public bool Tie { get; set; }
        public bool PlayerPair { get; set; }
        public bool BankerPair { get; set; }
    }

    public class BaccaratXMultiplierCardEntity : BlockchainBaccaratCard
    {
        public byte Value { get; set; }
    }
}