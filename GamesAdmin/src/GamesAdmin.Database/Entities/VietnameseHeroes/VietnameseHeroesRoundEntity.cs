using System;
using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("vietnameseheroes_rounds")]
    public class VietnameseHeroesRoundEntity : BaseRoundEntity
    {
        public decimal Bet { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal TotalAmount { get; set; }

        public IEnumerable<byte> Paylines { get; set; }

        public bool FreeSpin { get; set; }

        public IEnumerable<byte> Result { get; set; }

        public VietnameseHeroesSettlementResultEntity SettlementResult { get; set; }

        public IEnumerable<VietnameseHeroesBonusChestEntity> BonusChests { get; set; }

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

    public class VietnameseHeroesBonusChestEntity
    {
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Amount { get; set; }

        public bool Picked { get; set; }
    }

    public class VietnameseHeroesSettlementResultEntity
    {
        public IEnumerable<VietnameseHeroesWinningLineEntity> WinningPaylines { get; set; }

        public VietnameseHeroesBonusEntity Bonus { get; set; }

        public VietnameseHeroesFreeSpinEntity FreeSpins { get; set; }

        public VietnameseHeroesJackpotEntity Jackpot { get; set; }
    }

    public class VietnameseHeroesJackpotEntity
    {
        public byte WinningLine { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal WinningAmount { get; set; }
    }

    public class VietnameseHeroesWinningLineEntity
    {
        public byte Payline { get; set; }

        public string Symbol { get; set; }

        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class VietnameseHeroesBonusEntity
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class VietnameseHeroesFreeSpinEntity
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }
}