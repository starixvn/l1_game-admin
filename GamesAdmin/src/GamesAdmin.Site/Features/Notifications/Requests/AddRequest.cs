using GamesAdmin.Core.Enumeration;
using MediatR;

namespace GamesAdmin.Site.Features.Notifications.Requests
{
    public class AddRequest : IRequest<bool>
    {
        public EventScheduleType EventType { get; set; }

        public NotificationDisplayKind DisplayKind { get; set; }

        public string Message { get; set; }
    }
}
