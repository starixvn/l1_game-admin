using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("fourguardians_rounds")]
    public class FourGuardiansRoundEntity : BaseRoundEntity
    {
        public decimal Bet { get; set; }

        public decimal TotalAmount { get; set; }

        public IEnumerable<byte> Paylines { get; set; }

        public bool FreeSpin { get; set; }

        public IEnumerable<byte> Results { get; set; }

        public FourGuardiansSettlementResultEntity SettlementResult { get; set; }

        public IEnumerable<FourGuardiansBonusChestEntity> BonusChests { get; set; }

        public bool Paid { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal BetWin { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        public int? MemberId { get; set; }

        public string SiteId { get; set; }
    }

    public class FourGuardiansBonusChestEntity
    {
        public decimal Amount { get; set; }

        public bool Picked { get; set; }
    }

    public class FourGuardiansSettlementResultEntity
    {
        public IEnumerable<FourGuardiansWinningLineEntity> WinningPaylines { get; set; }

        public FourGuardiansBonusEntity Bonus { get; set; }

        public FourGuardiansFreeSpinEntity FreeSpins { get; set; }

        public FourGuardiansJackpotEntity Jackpot { get; set; }
    }

    public class FourGuardiansJackpotEntity
    {
        public byte WinningLine { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal WinningAmount { get; set; }
    }

    public class FourGuardiansWinningLineEntity
    {
        public byte Payline { get; set; }

        public string Symbol { get; set; }

        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class FourGuardiansBonusEntity
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class FourGuardiansFreeSpinEntity
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }
}