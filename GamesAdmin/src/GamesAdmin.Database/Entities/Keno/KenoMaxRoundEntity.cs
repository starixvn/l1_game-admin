using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.Keno
{
    [BsonCollection("kenomax_phiens")]
    public class KenoMaxRoundEntity : BaseKenoRoundEntity
    {
        public Dictionary<string, bool> SettlementResult { get; set; }
    }
}
