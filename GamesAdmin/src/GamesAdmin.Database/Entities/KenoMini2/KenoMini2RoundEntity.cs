using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.Keno
{
    [BsonCollection("kenomini2_phiens")]
    public class KenoMini2RoundEntity : BaseKenoRoundEntity
    {
        public Dictionary<string, bool> SettlementResult { get; set; }
    }
}
