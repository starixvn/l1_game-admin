using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.GameSettings.Requests;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings
{
    public class GamesHandler
        : IRequestHandler<EditRequest, EditViewModel>
        , IRequestHandler<UpdateRequest, bool>
        , IRequestHandler<AddRequest, AddViewModel>
        , IRequestHandler<CreateRequest, bool>
        , IRequestHandler<DeleteRequest, bool>
        , IRequestHandler<IsExistRequest, bool>
    {
        private readonly IGameService gameService;
        private readonly IMapper mapper;

        public GamesHandler(IMapper mapper, IGameService gameService)
        {
            this.mapper = mapper;
            this.gameService = gameService;
        }

        public async Task<EditViewModel> Handle(EditRequest request, CancellationToken cancellationToken)
        {
            GameConfig game;

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                game = (await gameService.GetGames()).FirstOrDefault();
            }
            else
            {
                game = await gameService.GetByName(request.Name);
            }

            return mapper.Map<EditViewModel>(game);
        }

        public Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        => gameService.Update(request.Game);

        public Task<AddViewModel> Handle(AddRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new AddViewModel());

        public Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        => gameService.Add(request.Game);

        public Task<bool> Handle(DeleteRequest request, CancellationToken cancellationToken)
        => gameService.Delete(request.Id);

        public Task<bool> Handle(IsExistRequest request, CancellationToken cancellationToken)
        => gameService.IsExistName(request.Name);
    }
}
