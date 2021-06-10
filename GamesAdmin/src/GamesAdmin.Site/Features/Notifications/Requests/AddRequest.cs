using GamesAdmin.Core.Enumeration;
using MediatR;

namespace GamesAdmin.Site.Features.Notifications.Requests
{
    public class AddRequest : IRequest<bool>
    {
        public byte DisplayKindValue { get; set; }

        public NotificationDisplayKind DisplayKind 
            => Enumeration.FromValue<NotificationDisplayKind>(DisplayKindValue.ToString());

        public string VnMessage { get; set; }

        public string EnMessage { get; set; }
    }
}
