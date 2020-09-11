using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameServers
{
    public class GameServerHandler
        : IRequestHandler<ReloadRequest, bool>
        , IRequestHandler<JWTRequest, JsonWebToken>
    {
        private readonly IGameServerService gameServerService;

        public GameServerHandler(IGameServerService gameServerService) 
        {
            this.gameServerService = gameServerService;
        }

        public async Task<bool> Handle(ReloadRequest request, CancellationToken cancellationToken)
        {
            await gameServerService.Reload();

            return true;
        }

        public Task<JsonWebToken> Handle(JWTRequest request, CancellationToken cancellationToken)
        => gameServerService.GetJWT(request);
    }
}
