using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("bac81_bets")]
    public class Bac81BetEntity : BaccaratXBetEntity
    {
    }

    public class BaccaratXBetEntity : BaseBetEntity
    {
        public decimal Banker { get; set; }

        public decimal BankerPair { get; set; }

        public decimal Player { get; set; }

        public decimal PlayerPair { get; set; }

        public decimal Tie { get; set; }

        public decimal Fee { get; set; }

        public Dictionary<string, IEnumerable<byte>> Multipliers { get; set; }
    }
}