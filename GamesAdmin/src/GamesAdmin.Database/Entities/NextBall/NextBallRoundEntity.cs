using System.Collections.Generic;
using GamesAdmin.Core.Models;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("luckynext1_rounds")]
    public class LuckyNext1RoundEntity : BaseRoundEntity
    {
        public long DrawId { get; set; }

        public List<NextBallRoundResult> Result { get; set; }

        public List<NextBallSettlementResult> SettlementResult { get; set; }
    }

    [BsonCollection("luckynext2_rounds")]
    public class LuckyNext2RoundEntity : LuckyNext1RoundEntity
    {
    }
}