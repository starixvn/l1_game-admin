using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("luckynext_rounds")]
    public class LuckyNextRoundEntity : LuckyNext1RoundEntity { }

    [BsonCollection("speedluckynext_rounds")]
    public class SpeedLuckyNextRoundEntity : LuckyNextRoundEntity { }

    [BsonCollection("vipluckynext_rounds")]
    public class VIPLuckyNextRoundEntity : LuckyNextRoundEntity { }
}