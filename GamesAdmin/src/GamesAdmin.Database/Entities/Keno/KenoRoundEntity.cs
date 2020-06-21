using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.Keno
{
    [BsonCollection("kenoproviet_phiens")]
    public class KenoRoundEntity: BaseRoundEntity
    {
        public byte[] Result { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }
}
