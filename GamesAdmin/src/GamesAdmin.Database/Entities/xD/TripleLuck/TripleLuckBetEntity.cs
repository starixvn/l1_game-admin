using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("tripleluck_bets")]
    public class TripleLuckBetEntity : BaseBetEntity { }

    [BsonCollection("speedtripleluck_bets")]
    public class SpeedTripleLuckBetEntity : TripleLuckBetEntity { }

    [BsonCollection("viptripleluck_bets")]
    public class VIPTripleLuckBetEntity : TripleLuckBetEntity { }
}