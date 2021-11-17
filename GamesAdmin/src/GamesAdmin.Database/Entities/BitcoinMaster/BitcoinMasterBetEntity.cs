using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("bitcoinmaster_betlogs")]
    public class BitcoinMasterBetEntity : BaseBetEntity
    {
        public decimal Amount { get; set; }

        public decimal Refund { get; set; }

        public string Choice { get; set; }

        public bool Win { get; set; }
    }
}