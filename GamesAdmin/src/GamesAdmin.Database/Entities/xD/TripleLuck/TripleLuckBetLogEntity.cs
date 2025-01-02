using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("tripleluck_betlogs")]
    public class TripleLuckBetLogEntity : BaseBetLogEntity { }

    [BsonCollection("speedtripleluck_betlogs")]
    public class SpeedTripleLuckBetLogEntity : TripleLuckBetLogEntity { }

    [BsonCollection("viptripleluck_betlogs")]
    public class VIPTripleLuckBetLogEntity : TripleLuckBetLogEntity { }
}