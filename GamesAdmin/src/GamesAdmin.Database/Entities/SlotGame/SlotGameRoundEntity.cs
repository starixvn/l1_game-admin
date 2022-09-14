using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class SlotGameRoundEntity : BaseRoundEntity
    {
        public decimal Bet { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal TotalAmount { get; set; }

        public IEnumerable<byte> Paylines { get; set; }

        public bool FreeSpin { get; set; }

        public IEnumerable<byte> Result { get; set; }

        public SlotGameSettlementResultEntity SettlementResult { get; set; }

        public IEnumerable<SlotGameBonusChestEntity> BonusChests { get; set; }

        public bool Paid { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal BetWin { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        public int? PlayerId { get; set; }

        public string SiteId { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime EndTime { get; set; }

        public string Currency { get; set; }

        public string OperatorId { get; set; }

        public decimal Rate { get; set; }
    }

    public class SlotGameBonusChestEntity
    {
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Amount { get; set; }

        public bool Picked { get; set; }
    }

    public class SlotGameSettlementResultEntity
    {
        public IEnumerable<SlotGameWinningLineEntity> WinningPaylines { get; set; }

        public SlotGameBonusEntity Bonus { get; set; }

        public SlotGameFreeSpinEntity FreeSpins { get; set; }
    }

    public class SlotGameJackpotEntity
    {
        public byte WinningLine { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal WinningAmount { get; set; }
    }

    public class SlotGameWinningLineEntity
    {
        public byte Payline { get; set; }

        public string Symbol { get; set; }

        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class SlotGameBonusEntity
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class SlotGameFreeSpinEntity
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }
}