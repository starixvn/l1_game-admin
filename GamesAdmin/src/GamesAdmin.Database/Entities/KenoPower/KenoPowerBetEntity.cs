using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class KenoPowerBetEntity : BaseBetEntity
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }

    [BsonCollection("kenopower_betlogs")]
    public class KenoPower1BetEntity : KenoPowerBetEntity
    {
    }

    [BsonCollection("kenopower2_betlogs")]
    public class KenoPower2BetEntity : KenoPowerBetEntity
    {
    }
}