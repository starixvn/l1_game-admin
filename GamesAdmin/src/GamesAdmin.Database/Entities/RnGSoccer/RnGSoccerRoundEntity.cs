using GamesAdmin.Database.Attributes;
using GamesAdmin.Database.Entities.DragonTigerFight;

namespace GamesAdmin.Database.Entities.RnGSoccer
{
    [BsonCollection("rngsoccer_rounds")]
    public class RnGSoccerRoundEntity : DragonTigerFightRoundEntity
    {
    }

    public class RnGSoccerResult : DragonTigerFightResult
    {
    }
}