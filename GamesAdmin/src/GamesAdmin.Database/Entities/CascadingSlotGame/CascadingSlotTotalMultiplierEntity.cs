using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class CascadingSlotTotalMultiplierEntity : MiniGameBaseEntity
    {
        public string PlayerId { get; set; }

        public string Name { get; set; }

        public bool Ended { get; set; }

        public IEnumerable<CascadingSlotRoundMultiplierEntity> Rounds { get; set; }
    }

    public class CascadingSlotRoundMultiplierEntity
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Time { get; set; }

        public long RoundId { get; set; }

        public int TotalMultiplier { get; set; }

        public int Multiplier { get; set; }
    }
}
