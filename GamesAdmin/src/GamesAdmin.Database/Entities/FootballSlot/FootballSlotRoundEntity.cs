using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("footballslot_rounds")]
    public class FootballSlotRoundEntity : BaseRoundEntity
    {
        public FootballSlotResultEntity Result { get; set; }
        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class FootballSlotResultEntity
    {
        public IEnumerable<string> Symbols { get; set; }
        public byte Number { get; set; }
    }
}