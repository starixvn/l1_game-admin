using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.Baccarat
{
    [BsonCollection("baccarat_cuocs")]
    public class BaccaratBetEntity : BaseBetEntity
    {
        public decimal Banker { get; set; }

        public decimal BankerPair { get; set; }

        public decimal Player { get; set; }

        public decimal PlayerPair { get; set; }

        public decimal Tie { get; set; }

        public decimal Big { get; set; }

        public decimal Small { get; set; }
    }
}