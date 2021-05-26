using System;
using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("threecards_phiens")]
    public class ThreeCardsRoundEntity : BaseRoundEntity
    {
        public IEnumerable<CardEntity> Result { get; set; }
        public ThreeCardsSettlementResultEntity SettlementResult { get; set; }
    }

    public class ThreeCardsSettlementResultEntity
    {
        public bool Red { get; set; }
        public bool Black { get; set; }
        public bool Diamonds { get; set; }
        public bool Hearts { get; set; }
        public bool Spades { get; set; }
        public bool Clubs { get; set; }
    }
}