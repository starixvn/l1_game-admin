using GamesAdmin.Core.Enumeration;
using MediatR;

namespace GamesAdmin.Api.Notifications.Requests
{
    public class UpdateRequest: IRequest<bool>
    {
        public string Id { get; set; }

        public string Message { get; set; }

        public EventScheduleType EventType { get; set; }

        public NotificationDisplayKind DisplayKind { get; set; }
    }
}
