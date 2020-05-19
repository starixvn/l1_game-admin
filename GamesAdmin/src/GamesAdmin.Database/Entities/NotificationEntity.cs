using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("notification")]
    public class NotificationEntity : MiniGameBaseEntity
    {
        public string Message { get; set; }

        public byte EventType { get; set; }

        public byte DisplayKind { get; set; }
    }
}
