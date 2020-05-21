using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Core.Models
{
    public class Notification
    {
        public Notification(            
            string message, 
            EventScheduleType eventType, 
            NotificationDisplayKind displayKind,
            string id = null) 
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
