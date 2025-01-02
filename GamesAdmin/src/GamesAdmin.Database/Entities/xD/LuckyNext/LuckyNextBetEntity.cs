using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("luckynext_bets")]
    public class LuckyNextBetEntity : LuckyNext1BetEntity { }

    [BsonCollection("speedluckynext_bets")]
    public class SpeedLuckyNextBetEntity : LuckyNextBetEntity { }

    [BsonCollection("vipluckynext_bets")]
    public class VIPLuckyNextBetEntity : LuckyNextBetEntity { }
}