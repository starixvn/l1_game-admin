using GamesAdmin.Core.Enumeration;
using Newtonsoft.Json;

namespace GamesAdmin.Core.Models
{
    public class Notification
    {
        public Notification() { }

        [JsonConstructor]
        public Notification(
            string id,
            string message,
            NotificationDisplayKind displayKind)
        {
            Id = id;
            Message = message;
            DisplayKind = displayKind;
        }

        public string Id { get; set; }

        public string Message { get; set; }

        public NotificationDisplayKind DisplayKind { get; set; }
    }
}
