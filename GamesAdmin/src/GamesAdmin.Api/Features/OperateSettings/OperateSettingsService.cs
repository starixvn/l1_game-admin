using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using GamesAdmin.Api.Games;
using System.Linq;
using GamesAdmin.Core.Models;
using GamesAdmin.Api.Features.OperateSettings.Models;

namespace GamesAdmin.Api.Features.OperateSettings
{
    public interface IOperateSettingsService
    {
        Task<OperateGameSettings> GetOperateGameSettingsAsync(IList<int> gameIds);
    }

    public class OperateSettingsService : IOperateSettingsService
    {
        private readonly IGameSettingsService gameSettingsService;

        public OperateSettingsService(IGameSettingsService gameSettingsService)
        {
            this.gameSettingsService = gameSettingsService;
        }

        public async Task<OperateGameSettings> GetOperateGameSettingsAsync(IList<int> gameIds)
        {
            var allGameSettings = await gameSettingsService.Get();
            var filterGames = gameIds != null && gameIds.Any() ? allGameSettings.Where(gs => gameIds.Contains(gs.Info.GameId)) : allGameSettings;

            return new OperateGameSettings
            {
                GameConfigs = filterGames.ToList()
            };
        }
    }
}