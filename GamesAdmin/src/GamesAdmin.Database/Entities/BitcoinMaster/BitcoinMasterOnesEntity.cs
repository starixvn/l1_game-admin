using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("bitcoinmaster_bets")]
    public class BitcoinMasterOnesEntity : BaseBetEntity
    {
        public decimal Refund { get; set; }

        public string Choice { get; set; }

        public decimal Up { get; set; }

        public decimal Down { get; set; }

        public decimal Odd { get; set; }

        public decimal Even { get; set; }
    }
}