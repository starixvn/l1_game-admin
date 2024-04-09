using GamesAdmin.Core.Models;
using GamesAdmin.Database.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("bacball2_rounds")]
    public class BacBall2RoundEntity : BaseRoundEntity
    {
        public long DrawId { get; set; }

        public List<BacBall2RoundResult> Result { get; set; }

        public List<BacBall2SettlementResult> SettlementResult { get; set; }
    }
    
}