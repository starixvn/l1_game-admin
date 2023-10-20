using GamesAdmin.Database.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("bandarqiuqiu_rounds")]
    public class BandarQiuQiuRoundEntity : BaseRoundEntity
    {
        public BandarQiuQiuRoundResultEntity Result { get; set; }

        public Dictionary<string, int> SettlementResult { get; set; }
    }

    public class BandarQiuQiuRoundResultEntity
    {
        public BandarQiuQiuResultItemEntity Dealer { get; set; }

        public BandarQiuQiuResultItemEntity Spade { get; set; }

        public BandarQiuQiuResultItemEntity Heart { get; set; }

        public BandarQiuQiuResultItemEntity Club { get; set; }

        public BandarQiuQiuResultItemEntity Diamond { get; set; }
    }

    public class BandarQiuQiuResultItemEntity 
    { 
        public List<string> Dominoes { get; set; }

        public int HandRankingId { get; set; }

        public IEnumerable<int> PipsOfSet { get; set; }
    }
}