using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("treasureisland_rounds")]
    public class TreasureIslandRoundEntity : CascadingSlotRoundEntity
    {
    }
}