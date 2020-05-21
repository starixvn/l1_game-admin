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
            EventScheduleType eventType,
            NotificationDisplayKind displayKind)
        {
            Id = id;
            Message = message;
            EventType = eventType;
            DisplayKind = displayKind;
        }

        public string Id { get; set; }

        public string Message { get; set; }

        public EventScheduleType EventType { get; set; }

        public NotificationDisplayKind DisplayKind { get; set; }
    }
}
