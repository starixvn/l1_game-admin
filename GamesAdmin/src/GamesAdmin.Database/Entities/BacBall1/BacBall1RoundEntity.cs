using GamesAdmin.Core.Models;
using GamesAdmin.Database.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("bacball1_rounds")]
    public class BacBall1RoundEntity : BaseRoundEntity
    {
        public long DrawId { get; set; }

        public List<BacBall1RoundResult> Result { get; set; }

        public List<BacBall1SettlementResult> SettlementResult { get; set; }
    }
    
}