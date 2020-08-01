using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.Keno
{
    [BsonCollection("kenosouth_phiens")]
    public class KenoSouthRoundEntity : BaseKenoRoundEntity
    {
        public Dictionary<string, bool> SettlementResult { get; set; }
    }
}
