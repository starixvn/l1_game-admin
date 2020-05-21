using GamesAdmin.Core.Enumeration;
using MediatR;

namespace GamesAdmin.Site.Features.Notifications.Requests
{
    public class AddRequest : IRequest<bool>
    {
        public byte EventTypeValue { get; set; }

        public byte DisplayKindValue { get; set; }

        public EventScheduleType EventType 
            => Enumeration.FromValue<EventScheduleType>(EventTypeValue.ToString());

        public NotificationDisplayKind DisplayKind 
            => Enumeration.FromValue<NotificationDisplayKind>(DisplayKindValue.ToString());

        public string Message { get; set; }
    }
}
