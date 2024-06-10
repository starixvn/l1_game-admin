using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class BaccaratXBetLogEntity : BaseBetLogEntity
    {
        public decimal Fee { get; set; }
    }

    [BsonCollection("bac81_betlogs")]
    public class Bac81BetLogEntity : BaccaratXBetLogEntity
    {
    }
}