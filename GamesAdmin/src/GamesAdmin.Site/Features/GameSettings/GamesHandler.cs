using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.GameSettings.Requests;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings
{
    public class GamesHandler
        : IRequestHandler<GamesRequest, GamesViewModel>,
        IRequestHandler<ToogleOnOffRequest, bool>
    {
        private readonly IGameService gameService;

        public GamesHandler(IGameService gameService) 
        {
            this.gameService = gameService;
        }

        public async Task<GamesViewModel> Handle(GamesRequest request, CancellationToken cancellationToken)
        => new GamesViewModel((await gameService.GetGames()).ToList());

        public async Task<bool> Handle(ToogleOnOffRequest request, CancellationToken cancellationToken)
        {
            var game = await gameService.Get(request.Id);

            var updatedGame = new Game(game.Id, game.Name, game.MinBet, game.MaxBet, !game.Enabled);

            return await gameService.Update(updatedGame);
        }
    }
}
