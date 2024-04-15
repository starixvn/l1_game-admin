using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("seball1_rounds")]
    public class SeBall1RoundEntity : BaseRoundEntity
    {
        public List<byte> Result { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }
}