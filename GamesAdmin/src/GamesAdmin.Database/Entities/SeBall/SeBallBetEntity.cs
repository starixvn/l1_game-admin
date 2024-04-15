using GamesAdmin.Database.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("seball1_bets")]
    public class SeBall1BetEntity : BaseBetEntity
    {
        public decimal Odd { get; set; }

        public decimal Even { get; set; }
        public decimal Red2 { get; set; }
        public decimal Red3 { get; set; }
        public decimal Red4 { get; set; }
        public decimal Blue3 { get; set; }
        public decimal Blue4 { get; set; }
        public double TotalAmount { get; set; }
        public Dictionary<string, decimal> Odds { get; set; }
    }
}