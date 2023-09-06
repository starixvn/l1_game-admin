using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("pontoon_bets")]
    public class PontoonBetEntity : BankingGameBetEntity
    {
    }
}