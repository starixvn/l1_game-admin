using GamesAdmin.Core.Enumeration;
using MediatR;

namespace GamesAdmin.Site.Features.Notifications.Requests
{
    public class AddRequest : IRequest<bool>
    {
        public byte DisplayKindValue { get; set; }

        public NotificationDisplayKind DisplayKind
            => Enumeration.FromValue<NotificationDisplayKind>(DisplayKindValue.ToString());

        public string VNMessage { get; set; }

        public string ENMessage { get; set; }

        public string CNMessage { get; set; }

        public string TWMessage { get; set; }

        public string THMessage { get; set; }

        public string IDMessage { get; set; }

        public string JAMessage { get; set; }

        public string ESMessage { get; set; }
    }
}
