using System;
using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("promotions")]
    public class PromotionEntity : MiniGameBaseEntity
    {
        public PromotionEntity()
        {
            Time = DateTime.UtcNow;
        }
        public int GameId { get; set; }
        public Dictionary<string, double> Odds { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Status { get; set; }
        public bool IsDaily { get; set; }
    }
}
