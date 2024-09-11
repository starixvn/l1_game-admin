using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Site._Shared.Services.GeneralGameSettings;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.GameGroupManagement.Requests;
using GamesAdmin.Site.Features.GameGroupManagement.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.GameGroupManagement.Handlers
{
    public class GameGroupHandler :
        IRequestHandler<CreatePageRequest, CreateGameGroupViewModel>,
        IRequestHandler<CreateRequest, MessageResult>
    {
        private readonly IGeneralGameSettingService generalGameSettingService;
        private readonly IGameGroupManagementApi gameGroupManagementApi;

        public GameGroupHandler(IGeneralGameSettingService generalGameSettingService, IGameGroupManagementApi gameGroupManagementApi)
        {
            this.generalGameSettingService = generalGameSettingService;
            this.gameGroupManagementApi = gameGroupManagementApi;
        }

        public async Task<CreateGameGroupViewModel> Handle(CreatePageRequest request, CancellationToken cancellationToken)
        {
            var games = await generalGameSettingService.GetAllGames();

            return new CreateGameGroupViewModel
            {
                Games = games.Select(x => new SelectListItem(x.DisplayName ?? Game.FromGameName(x.Name).DisplayName, x.GameId.ToString()))
            };
        }

        public async Task<MessageResult> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            var isSuccessfull = await gameGroupManagementApi.Create(new GameGroup 
            {
                Name = request.Model.Name,
                GameIds = request.Model.GameIds
            });

            return new MessageResult(isSuccessfull);
        }
    }
}