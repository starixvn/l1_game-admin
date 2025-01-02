using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("luckynext_betlogs")]
    public class LuckyNextBetLogEntity : LuckyNext1BetLogEntity { }

    [BsonCollection("speedluckynext_betlogs")]
    public class SpeedLuckyNextBetLogEntity : LuckyNextBetLogEntity { }

    [BsonCollection("speedluckynext_betlogs")]
    public class VIPLuckyNextBetLogEntity : LuckyNextBetLogEntity { }
}