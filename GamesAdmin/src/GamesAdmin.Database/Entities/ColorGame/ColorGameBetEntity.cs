using GamesAdmin.Database.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("colorgame_bets")]
    public class ColorGameBetEntity : BaseBetEntity
    {
        public decimal Yellow { get; set; }

        public decimal Red { get; set; }

        public decimal Pink { get; set; }

        public decimal White { get; set; }

        public decimal Blue { get; set; }

        public decimal Green { get; set; }

        public decimal Hot { get; set; }

        public decimal Cold { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
        
        public double TotalAmount { get; set; }
    }
}