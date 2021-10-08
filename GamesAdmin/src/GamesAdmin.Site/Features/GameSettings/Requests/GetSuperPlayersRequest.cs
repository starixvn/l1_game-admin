using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class GetSuperPlayersRequest : IRequest<SuperPlayersViewModel>
    {
    }
}
