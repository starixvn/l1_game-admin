using System;
using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("notifications")]
    public class NotificationEntity : MiniGameBaseEntity
    {
        public NotificationEntity()
        {
            Time = DateTime.UtcNow;
        }

        public string Message { get; set; }

        public byte DisplayKind { get; set; }

        public string GameName { get; set; }

        public Dictionary<string, string> Messages { get; set; }

        public Dictionary<string, string> Title { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string PromotionId { get; set; }

        public bool? Enabled { get; set; }

        public byte? GameId { get; set; }
    }
}