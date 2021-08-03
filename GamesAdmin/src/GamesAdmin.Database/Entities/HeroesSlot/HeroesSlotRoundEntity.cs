using System;
using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("heroesslot_rounds")]
    public class HeroesSlotRoundEntity : BaseRoundEntity
    {
        public decimal Bet { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal TotalAmount { get; set; }

        public IEnumerable<byte> Paylines { get; set; }

        public bool FreeSpin { get; set; }

        public IEnumerable<byte> Result { get; set; }

        public HeroesSlotSettlementResultEntity SettlementResult { get; set; }

        public IEnumerable<HeroesSlotBonusChestEntity> BonusChests { get; set; }

        public bool Paid { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal BetWin { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        public int? MemberId { get; set; }

        public string SiteId { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime EndTime { get; set; }
    }

    public class HeroesSlotBonusChestEntity
    {
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Amount { get; set; }

        public bool Picked { get; set; }
    }

    public class HeroesSlotSettlementResultEntity
    {
        public IEnumerable<HeroesSlotWinningLineEntity> WinningPaylines { get; set; }

        public HeroesSlotBonusEntity Bonus { get; set; }

        public HeroesSlotFreeSpinEntity FreeSpins { get; set; }

        public HeroesSlotJackpotEntity Jackpot { get; set; }
    }

    public class HeroesSlotJackpotEntity
    {
        public byte WinningLine { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal WinningAmount { get; set; }
    }

    public class HeroesSlotWinningLineEntity
    {
        public byte Payline { get; set; }

        public string Symbol { get; set; }

        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class HeroesSlotBonusEntity
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class HeroesSlotFreeSpinEntity
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }
}