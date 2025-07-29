using GamesAdmin.Core.Enumeration;
using MediatR;

namespace GamesAdmin.Site.Features.Notifications.Requests
{
    public class UpdateRequest : IRequest<bool>
    {
        public string Id { get; set; }

        public byte DisplayKindValue { get; set; }

        public string VNMessage { get; set; }

        public string ENMessage { get; set; }

        public string CNMessage { get; set; }

        public string TWMessage { get; set; }

        public string THMessage { get; set; }

        public string IDMessage { get; set; }

        public string JAMessage { get; set; }

        public string ESMessage { get; set; }

        public NotificationDisplayKind DisplayKind
            => Enumeration.FromValue<NotificationDisplayKind>(DisplayKindValue.ToString());
    }
}
