using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("blackjack_rounds")]
    public class PontoonRoundEntity : BankingGameRoundEntity
    {
    }
}