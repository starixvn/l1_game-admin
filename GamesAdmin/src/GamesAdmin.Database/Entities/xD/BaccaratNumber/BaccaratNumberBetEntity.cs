using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("baccaratnumber_bets")]
    public class BaccaratNumberBetEntity : BacBall1BetEntity { }

    [BsonCollection("speedbaccaratnumber_bets")]
    public class SpeedBaccaratNumberBetEntity : BaccaratNumberBetEntity { }

    [BsonCollection("vipbaccaratnumber_bets")]
    public class VIPBaccaratNumberBetEntity : BaccaratNumberBetEntity { }
}