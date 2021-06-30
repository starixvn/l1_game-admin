using System.Collections.Generic;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateDisabledMessageRequest: IRequest<bool>
    {
        public UpdateDisabledMessageRequest(string name, Dictionary<string, string> messages) 
        {
            Name = name;
            Messages = messages;
        }

        public string Name { get; }

        public Dictionary<string, string> Messages { get; }
    }
}
