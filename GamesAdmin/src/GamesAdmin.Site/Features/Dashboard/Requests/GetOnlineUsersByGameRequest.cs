using GamesAdmin.Site.Features.Dashboard.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Dashboard.Requests
{
    public class GetOnlineUsersByGameRequest : IRequest<OnlineUsersViewModel>
    {
        public GetOnlineUsersByGameRequest(string game)
        {
            Game = game;
        }

        public string Game { get; }
    }
}
