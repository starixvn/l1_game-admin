using GamesAdmin.Site.Features.Dashboard.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Dashboard.Requests
{
    public class GetOnlineUsersRequest : IRequest<OnlineUsersViewModel>
    {
        public GetOnlineUsersRequest(string jwtToken = "")
        {
            JwtToken = jwtToken;
        }

        public string JwtToken { get; }
    }
}