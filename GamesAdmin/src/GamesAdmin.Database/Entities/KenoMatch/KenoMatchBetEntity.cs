using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class KenoMatchBetEntity : BaseBetEntity
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }

    [BsonCollection("kenosoccer_betlogs")]
    public class KenoMatch1BetEntity : KenoMatchBetEntity
    {
    }

    [BsonCollection("kenosoccer2_betlogs")]
    public class KenoMatch2BetEntity : KenoMatchBetEntity
    {
    }
}