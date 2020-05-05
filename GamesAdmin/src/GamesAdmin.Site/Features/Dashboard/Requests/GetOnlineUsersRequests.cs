using GamesAdmin.Site.Features.Dashboard.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Dashboard.Requests
{
    public class GetOnlineUsersRequests : IRequest<OnlineUsersViewModel>
    {
    }
}