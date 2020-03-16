using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Games.Requests;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Games
{
    public class GameHandler
        : IRequestHandler<GetAllRequest, IEnumerable<Game>>
        , IRequestHandler<GetByIdRequest, Game>
        , IRequestHandler<CreateRequest, bool>
        , IRequestHandler<UpdateRequest, bool>
        , IRequestHandler<DeleteRequest, bool>
    {
        private readonly IGameService gameService;

        public GameHandler(IGameService gamesService)
        {
            this.gameService = gamesService;
        }

        public Task<IEnumerable<Game>> Handle(GetAllRequest request, CancellationToken cancellationToken)
         => gameService.Get();

        public Task<Game> Handle(GetByIdRequest request, CancellationToken cancellationToken)
         => gameService.Get(request.Id);

        public Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        => gameService.Create(request.Game);

        public Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        => gameService.Update(request.Game);

        public Task<bool> Handle(DeleteRequest request, CancellationToken cancellationToken)
        => gameService.Delete(request.Id);
    }
}
