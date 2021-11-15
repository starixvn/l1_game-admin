using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class KenoMatchBetEntity : BaseBetEntity
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }

    [BsonCollection("kenomatch1_cuocs")]
    public class KenoMatch1BetEntity : KenoMatchBetEntity
    {
    }

    [BsonCollection("kenomatch2_cuocs")]
    public class KenoMatch2BetEntity : KenoMatchBetEntity
    {
    }
}