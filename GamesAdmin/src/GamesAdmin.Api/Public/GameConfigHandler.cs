using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Public.Models;
using GamesAdmin.Api.Public.Requests;
using MediatR;

namespace GamesAdmin.Api.Public
{
    public class GameConfigHandler
        : IRequestHandler<GetGameConfigRequest, GameConfig>
    {
        private readonly IGameConfigService gameConfigService;

        public GameConfigHandler(IGameConfigService gameConfigService)
        {
            this.gameConfigService = gameConfigService;
        }

        public Task<GameConfig> Handle(GetGameConfigRequest request, CancellationToken cancellationToken)
         => gameConfigService.GetById(request.GameId);
    }
}