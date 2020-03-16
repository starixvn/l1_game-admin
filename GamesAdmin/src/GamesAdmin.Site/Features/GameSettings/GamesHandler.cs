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
        : IRequestHandler<IndexRequest, IndexViewModel>,
        IRequestHandler<ToogleOnOffRequest, bool>,
        IRequestHandler<EditRequest, EditViewModel>,
        IRequestHandler<UpdateRequest, bool>,
        IRequestHandler<AddRequest, AddViewModel>,
        IRequestHandler<CreateRequest, bool>,
        IRequestHandler<DeleteRequest, bool>
    {
        private readonly IGameService gameService;

        public GamesHandler(IGameService gameService)
        {
            this.gameService = gameService;
        }

        public async Task<IndexViewModel> Handle(IndexRequest request, CancellationToken cancellationToken)
        => new IndexViewModel((await gameService.GetGames()).ToList());

        public async Task<bool> Handle(ToogleOnOffRequest request, CancellationToken cancellationToken)
        {
            //TODO remove
            var game = await gameService.Get(request.Id);

            var updatedGame = new Game(game.Id, game.Name, game.MinBet, game.MaxBet, !game.Enabled);

            return await gameService.Update(updatedGame);
        }

        public async Task<EditViewModel> Handle(EditRequest request, CancellationToken cancellationToken)
        {
            var game = await gameService.Get(request.Id);

            return new EditViewModel
            {
                Id = game.Id,
                Name = game.Name,
                MinBet = game.MinBet,
                MaxBet = game.MaxBet,
                Enabled = game.Enabled
            };
        }

        public Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        => gameService.Update(request.Game);

        public Task<AddViewModel> Handle(AddRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new AddViewModel());

        public Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        => gameService.Add(request.Game);

        public Task<bool> Handle(DeleteRequest request, CancellationToken cancellationToken)
        => gameService.Delete(request.Id);
    }
}
