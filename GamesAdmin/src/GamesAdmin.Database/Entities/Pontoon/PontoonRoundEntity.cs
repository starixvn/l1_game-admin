using System;
using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("pontoon_rounds")]
    public class PontoonRoundEntity : BaseRoundEntity
    {
        public IEnumerable<PontoonCardEntity> Result { get; set; }

        public DateTime RoundStartTime { get; set; }

        public bool Ended { get; set; }

        public string Uid { get; set;}

        public string Name { get; set; }

        public byte SettlementResult { get; set; }
    }

    public class PontoonCardEntity
    {
        public string Rank { get; set; }

        public byte Suit { get; set; }
    }
}