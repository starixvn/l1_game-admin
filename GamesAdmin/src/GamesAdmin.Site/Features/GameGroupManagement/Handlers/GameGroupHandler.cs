using System.Collections.Generic;
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
        IRequestHandler<CreateRequest, MessageResult>,
        IRequestHandler<IndexPageRequet, GameGroupManagementViewModel>,
        IRequestHandler<UpdatePageRequest, UpdateGameGroupViewModel>,
        IRequestHandler<UpdateRequest, MessageResult>
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
            var gameGroups = await gameGroupManagementApi.GetAll();

            return new CreateGameGroupViewModel
            {
                Games = FilterGames(games, gameGroups.SelectMany(x => x.GameIds))
                    .Select(x => new SelectListItem(x.DisplayName ?? Game.FromGameName(x.Name).DisplayName, x.GameId.ToString()))
            };
        }

        public async Task<MessageResult> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            var isSuccessfull = await gameGroupManagementApi.Create(new GameGroup
            {
                Name = request.Model.Name,
                GameIds = request.Model.GameIds
            });

            return new MessageResult(isSuccessfull, "Success!");
        }

        public async Task<GameGroupManagementViewModel> Handle(IndexPageRequet request, CancellationToken cancellationToken)
        {
            var gameGroups = await gameGroupManagementApi.GetAll();

            return new GameGroupManagementViewModel
            {
                GameGroups = gameGroups.Select(x => new GameGroupViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    GameIds = x.GameIds
                })
            };
        }
        public async Task<UpdateGameGroupViewModel> Handle(UpdatePageRequest request, CancellationToken cancellationToken)
        {
            var games = await generalGameSettingService.GetAllGames();
            var gameGroups = (await gameGroupManagementApi.GetAll()).ToList();
            var gameGroup = gameGroups.FirstOrDefault(x => x.Id == request.Id);

            return new UpdateGameGroupViewModel
            {
                Id = gameGroup?.Id,
                Name = gameGroup?.Name,
                GameIds = gameGroup?.GameIds,
                Games = FilterGames(games, gameGroups.SelectMany(x => x.GameIds).Where(x => !gameGroup.GameIds.Contains(x)))
                    .Select(x => new SelectListItem(x.DisplayName ?? Game.FromGameName(x.Name).DisplayName, x.GameId.ToString()))
            };
        }

        public async Task<MessageResult> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            var isSuccessfull = await gameGroupManagementApi.Update(new GameGroup
            {
                Id = request.Model.Id,
                Name = request.Model.Name,
                GameIds = request.Model.GameIds
            });

            return new MessageResult(isSuccessfull, "Success!");
        }

        private IEnumerable<GeneralGameSetting> FilterGames(IEnumerable<GeneralGameSetting> games, IEnumerable<int> gameIdsInGroups)
        {
            return games.Where(x => !gameIdsInGroups.Contains(x.GameId));
        }
    }
}