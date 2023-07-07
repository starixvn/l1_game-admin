using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.DragonTigerFight
{
    [BsonCollection("dragontigerfight_rounds")]
    public class DragonTigerFightRoundEntity : BaseRoundEntity
    {
        public DragonTigerFightResult Result { get; set; }
        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class DragonTigerFightResult
    {
        public BlockchainBaccaratCard Dragon { get; set; }

        public BlockchainBaccaratCard Tiger { get; set; }
    }
}