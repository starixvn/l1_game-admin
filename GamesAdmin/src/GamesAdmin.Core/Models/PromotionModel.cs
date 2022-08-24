using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Core.Models
{
    public class PromotionModel
    {
        public string PromotionId { get; set; }
        public GameId GameId { get; set; }
        public Dictionary<string, double> Odds { get; set; }
        public DateTime StartDateTime { get; set; } = DateTime.Now;
        public DateTime EndDateTime { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public bool IsDaily { get; set; }
        public bool HasAnnouncement { get; set; }
        public DateTime Time { get; } = DateTime.UtcNow;
    }
}
