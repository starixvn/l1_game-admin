using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class CascadingSlotRoundEntity : BaseRoundEntity
    {
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal TotalAmount { get; set; }

        public byte BetLevel { get; set; }

        public decimal CoinValue { get; set; }

        public byte BetMultiplier { get; set; }

        public bool FreeSpin { get; set; }

        public string Result { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Betwin { get; set; }

        public byte TotalMultiplier { get; set; }

        public byte Multiplier { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal TotalTumbleWin { get; set; }

        public bool Paid { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        public string SiteId { get; set; }

        public int PlayerId { get; set; }

        public string OperatorId { get; set; }

        public string Currency { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime EndTime { get; set; }

        public decimal Rate { get; set; }

        public CascadingSlotSettlementResultEntity SettlementResult { get; set; }
    }

    public class CascadingSlotSettlementResultEntity
    {
        public IEnumerable<CascadingSlotTumbleEntity> Tumbles { get; set; }

        public SlotGameFreeSpinEntity FreeSpins { get; set; }
    }

    public class CascadingSlotTumbleEntity
    {
        public Dictionary<string, CascadingSlotWinningSymbolEntity> Icons { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Win { get; set; }

        public string OriginalBoard { get; set; }

        public IEnumerable<byte> Multipliers { get; set; }
    }

    public class CascadingSlotWinningSymbolEntity
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }

        public decimal Win { get; set; }
    }
}
