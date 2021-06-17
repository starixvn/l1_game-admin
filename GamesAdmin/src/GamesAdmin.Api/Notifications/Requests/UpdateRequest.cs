using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using MediatR;

namespace GamesAdmin.Api.Notifications.Requests
{
    public class UpdateRequest : IRequest<bool>
    {
        public UpdateRequest() { }

        public UpdateRequest(Dictionary<string, string> messages, NotificationDisplayKind displayKind, string gameName = "")
        {
            Messages = messages;
            DisplayKind = displayKind;
            GameName = gameName;
        }

        public string Id { get; set; }

        public NotificationDisplayKind DisplayKind { get; set; }

        public string GameName { get; set; }

        public Dictionary<string, string> Messages { get; set; }
    }
}