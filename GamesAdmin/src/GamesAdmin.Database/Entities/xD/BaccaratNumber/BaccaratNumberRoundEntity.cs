using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("baccaratnumber_rounds")]
    public class BaccaratNumberRoundEntity : BacBall1RoundEntity { }

    [BsonCollection("speedbaccaratnumber_rounds")]
    public class SpeedBaccaratNumberRoundEntity : BaccaratNumberRoundEntity { }


    [BsonCollection("vipbaccaratnumber_rounds")]
    public class VIPBaccaratNumberRoundEntity : BaccaratNumberRoundEntity { }
}