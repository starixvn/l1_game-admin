using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.GameSettings.Requests;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings
{
    public class GamesHandler
        : IRequestHandler<GamesRequest, GamesViewModel>
    {
        private readonly IGameService gameService;

        public GamesHandler(IGameService gameService) 
        {
            this.gameService = gameService;
        }

        public async Task<GamesViewModel> Handle(GamesRequest request, CancellationToken cancellationToken)
        => new GamesViewModel((await gameService.GetGames()).ToList());
    }
}
