using GamesAdmin.Database.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("bandarqiuqiu_bets")]
    public class BandarQiuQiuBetEntity : BaseBetEntity
    {
        public decimal Spade { get; set; }

        public decimal Heart { get; set; }

        public decimal Club { get; set; }

        public decimal Diamond { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
        
        public double TotalAmount { get; set; }
    }
}