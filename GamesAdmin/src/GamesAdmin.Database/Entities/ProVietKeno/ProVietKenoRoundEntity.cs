using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("kenoproviet_phiens")]
    public class ProVietKenoRoundEntity : BaseRoundEntity
    {
        public IEnumerable<byte> Result { get; set; }

        public SettlementResult SettlementResult { get; set; }

        public bool Settled { get; set; }
    }

    public class SettlementResult
    {
        public bool Big { get; set; }
        public bool Small { get; set; }
        public bool Odd { get; set; }
        public bool Even { get; set; }
        public bool Gold { get; set; }
        public bool Wood { get; set; }
        public bool Water { get; set; }
        public bool Fire { get; set; }
        public bool Earth { get; set; }
    }
}