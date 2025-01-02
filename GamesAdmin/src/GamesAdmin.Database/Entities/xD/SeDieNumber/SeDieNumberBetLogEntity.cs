using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("sedienumber_betlogs")]
    public class SeDieNumberBetLogEntity : BaseBetLogEntity { }

    [BsonCollection("speedsedienumber_betlogs")]
    public class SpeedSeDieNumberBetLogEntity : SeDieNumberBetLogEntity { }

    [BsonCollection("vipsedienumber_betlogs")]
    public class VIPSeDieNumberBetLogEntity : SeDieNumberBetLogEntity { }
}