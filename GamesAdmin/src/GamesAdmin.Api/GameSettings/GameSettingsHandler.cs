using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Games.Requests;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Games
{
    public class GameSettingsHandler
        : IRequestHandler<GetAllRequest, IEnumerable<GameConfig>>
        , IRequestHandler<CreateRequest, bool>
        , IRequestHandler<UpdateRequest, bool>
        , IRequestHandler<DeleteRequest, bool>
        , IRequestHandler<GetByNameRequest, GameConfig>
    {
        private readonly IGameSettingsService gameService;

        public GameSettingsHandler(IGameSettingsService gamesService)
        {
            this.gameService = gamesService;
        }

        public Task<IEnumerable<GameConfig>> Handle(GetAllRequest request, CancellationToken cancellationToken)
         => gameService.Get();

        public Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        => gameService.Create(request.Game);

        public Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        => gameService.Update(request.Game);

        public Task<bool> Handle(DeleteRequest request, CancellationToken cancellationToken)
        => gameService.Delete(request.Name);

        public Task<GameConfig> Handle(GetByNameRequest request, CancellationToken cancellationToken)
        => gameService.GetByName(request.Name);
    }
}
