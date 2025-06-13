using GamesAdmin.Database.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("miniroulette_rounds")]
    public class MiniRouletteRoundEntity : BaseRoundEntity
    {
        public byte Result { get; set; }

        public MiniRouletteSettlementResultEntity SettlementResult { get; set; }
    }

    public class MiniRouletteSettlementResultEntity : RouletteSettlementResultEntity { }
}
