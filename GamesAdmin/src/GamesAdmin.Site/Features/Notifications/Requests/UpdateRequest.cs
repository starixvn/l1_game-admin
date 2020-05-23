using GamesAdmin.Core.Enumeration;
using MediatR;

namespace GamesAdmin.Site.Features.Notifications.Requests
{
    public class UpdateRequest : IRequest<bool>
    {
        public string Id { get; set; }

        public string Message { get; set; }

        public byte DisplayKindValue { get; set; }

        public NotificationDisplayKind DisplayKind
            => Enumeration.FromValue<NotificationDisplayKind>(DisplayKindValue.ToString());
    }
}
