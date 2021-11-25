using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("bitcoinmaster_rounds")]
    public class BitcoinMasterRoundEntity : BaseRoundEntity
    {
        public BitcoinMasterRoundResultEntity Result { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class BitcoinMasterRoundResultEntity
    {
        [BsonElement("open")]
        public decimal BettingPrice { get; set; }

        [BsonElement("close")]
        public decimal ResultPrice { get; set; }
    }
}