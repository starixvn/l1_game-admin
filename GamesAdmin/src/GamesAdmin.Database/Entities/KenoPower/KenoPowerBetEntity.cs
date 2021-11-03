using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class KenoPowerBetEntity : BaseBetEntity
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }

    [BsonCollection("kenopower1_cuocs")]
    public class KenoPower1BetEntity : KenoPowerBetEntity
    {
    }

    [BsonCollection("kenopower2_cuocs")]
    public class KenoPower2BetEntity : KenoPowerBetEntity
    {
    }
}