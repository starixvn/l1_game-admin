using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("luckynext1_betlogs")]
    public class LuckyNext1BetLogEntity : BaseBetLogEntity
    {
    }

    [BsonCollection("luckynext2_betlogs")]
    public class LuckyNext2BetLogEntity : LuckyNext1BetLogEntity
    {
    }
}