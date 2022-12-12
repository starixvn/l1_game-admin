using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("winthreecards_betlogs")]
    public class WinThreeCardsBetLogEntity : UnbalancedBigSmallBetLogEntity
    {
    }
}