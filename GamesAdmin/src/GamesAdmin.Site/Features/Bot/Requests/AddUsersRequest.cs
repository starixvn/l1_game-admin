using System.Collections.Generic;
using GamesAdmin.Site.Features.Bot.Results;
using MediatR;

namespace GamesAdmin.Site.Features.Bot.Requests
{
    public class AddUsersRequest : IRequest<List<AddBotResult>>
    {
        public AddUsersRequest(string textNames) 
        {
            TextNames = textNames;
        }

        public string TextNames { get; }
    }
}
