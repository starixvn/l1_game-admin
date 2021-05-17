using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using MediatR;

namespace GamesAdmin.Api.Notifications.Requests
{
    public class AddRequest : IRequest<bool>
    {
        public string Message { get; set; }

        public NotificationDisplayKind DisplayKind { get; set; }

        public string GameName { get; set; }

        public Dictionary<string, string> Messages { get; set; }
    }
}