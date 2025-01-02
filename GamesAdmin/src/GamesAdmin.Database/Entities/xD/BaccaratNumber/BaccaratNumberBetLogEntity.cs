using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("baccaratnumber_betlogs")]
    public class BaccaratNumberBetLogEntity : BacBall1BetLogEntity { }

    [BsonCollection("speedbaccaratnumber_betlogs")]
    public class SpeedBaccaratNumberBetLogEntity : BaccaratNumberBetLogEntity { }

    [BsonCollection("vipbaccaratnumber_betlogs")]
    public class VIPBaccaratNumberBetLogEntity : BaccaratNumberBetLogEntity { }
}