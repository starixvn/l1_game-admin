using System;
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
    }
}
