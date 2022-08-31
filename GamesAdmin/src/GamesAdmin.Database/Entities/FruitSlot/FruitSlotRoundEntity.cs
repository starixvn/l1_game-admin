using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("fruitslot_rounds")]
    public class FruitSlotRoundEntity : BaseRoundEntity
    {
        public FruitSlotResultEntity Result { get; set; }
        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class FruitSlotResultEntity
    {
        public IEnumerable<string> Symbols { get; set; }
        public byte Number { get; set; }
    }
}