using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Core.Models
{
    public class Notification
    {
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

        public string Id { get; }

        public string Message { get; }

        public EventScheduleType EventType { get; }

        public NotificationDisplayKind DisplayKind { get; }
    }
}
