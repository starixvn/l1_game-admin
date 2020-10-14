using GamesAdmin.Site.Features.GameUsers.Results;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.GameUsers.Requests
{
    public class AddUsersRequest : IRequest<List<AddUsersResult>>
    {
        public AddUsersRequest(string textNames) 
        {
            TextNames = textNames;
        }

        public string TextNames { get; }
    }
}
