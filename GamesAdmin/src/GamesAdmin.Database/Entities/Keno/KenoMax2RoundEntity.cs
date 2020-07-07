using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.Keno
{
    [BsonCollection("kenomax2_phiens")]
    public class KenoMax2RoundEntity : BaseKenoRoundEntity
    {
        public Dictionary<string, bool> SettlementResult { get; set; }
    }
}
