using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("bitcoinmaster_betlogs")]
    public class BitcoinMasterBetEntity : BaseBetEntity
    {
        public string Choice { get; set; }

        public bool Win { get; set; }

        public decimal Amount { get; set; }

        public decimal Refund { get; set; }
    }
}