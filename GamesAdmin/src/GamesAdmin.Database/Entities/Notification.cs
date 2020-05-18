using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("notification")]
    public class Notification : MiniGameBaseEntity
    {
        public string Message { get; set; }

        public int Type { get; set; }

        public int DisplayKind { get; set; }
    }
}
