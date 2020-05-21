using GamesAdmin.Core.Enumeration;
using MediatR;

namespace GamesAdmin.Api.Notifications.Requests
{
    public class AddRequest : IRequest<bool>
    {
        public AddRequest(
            string message,
            EventScheduleType eventType,
            NotificationDisplayKind displayKind)
        {
            Message = message;
            EventType = eventType;
            DisplayKind = displayKind;
        }

        public string Message { get; }

        public EventScheduleType EventType { get; }

        public NotificationDisplayKind DisplayKind { get; }
    }
}
