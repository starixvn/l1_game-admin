using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("sicbonumber_bets")]
    public class SicBoNumberBetEntity : BaseBetEntity { }

    [BsonCollection("speedsicbonumber_bets")]
    public class SpeedSicBoNumberBetEntity : SicBoNumberBetEntity { }

    [BsonCollection("vipsicbonumber_bets")]
    public class VIPSicBoNumberBetEntity : SicBoNumberBetEntity { }
}