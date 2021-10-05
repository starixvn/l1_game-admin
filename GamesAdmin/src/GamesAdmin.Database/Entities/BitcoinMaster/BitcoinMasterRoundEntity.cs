using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("bitcoinmaster_phiens")]
    public class BitcoinMasterRoundEntity : BaseRoundEntity
    {
        public BitcoinMasterRoundResultEntity Result { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class BitcoinMasterRoundResultEntity
    {
        public decimal BettingPrice { get; set; }

        public decimal ResultPrice { get; set; }
    }
}