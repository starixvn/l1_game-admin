using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.Keno
{
    [BsonCollection("kenonorth_phiens")]
    public class KenoNorthRoundEntity : BaseKenoRoundEntity
    {
        public Dictionary<string, bool> SettlementResult { get; set; }
    }
}
