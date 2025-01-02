using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("sicball_betlogs")]
    public class SicBoNumberBetLogEntity : BaseBetLogEntity { }
    
    [BsonCollection("speedsicball_betlogs")]
    public class SpeedSicBoNumberBetLogEntity : SicBoNumberBetLogEntity { }
    
    [BsonCollection("vipsicball_betlogs")]
    public class VIPSicBoNumberBetLogEntity : SicBoNumberBetLogEntity { }
}