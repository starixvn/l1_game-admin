using GamesAdmin.Site.Features.GameUsers.Results;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.GameUsers.Requests
{
    public class AddUsersRequest : IRequest<List<AddUsersResult>>
    {
        public AddUsersRequest(string textNames, bool isBot) 
        {
            TextNames = textNames;
            IsBot = isBot;
        }

        public string TextNames { get; }

        public bool IsBot { get; }
    }
}
