using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("pontoon_rounds")]
    public class PontoonRoundEntity : BankingGameRoundEntity
    {
    }
}