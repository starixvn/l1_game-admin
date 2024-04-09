using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.GameManagementApi;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.GeneralGameSettings
{
    public interface IGeneralGameSettingService
    {
        Task<IEnumerable<GeneralGameSetting>> GetAll();

        Task<string> GetGameName(int gameId);

        Task<int> GetGameId(string name);

        Task<byte> GetGameType(int gameId);

        Task<string> GetDisplayName(int gameId);

        Task<bool> IsCrashGame(int gameId);
    }

    public class GeneralGameSettingService : IGeneralGameSettingService
    {
        private readonly IGenericRepository<GameConfigEntity> repository;
        private readonly IServiceProvider serviceProvider;
        private readonly IAppSettings appSettings;

        public GeneralGameSettingService(IGenericRepository<GameConfigEntity> repository, IServiceProvider serviceProvider, IAppSettings appSettings)
        {
            this.repository = repository;
            this.serviceProvider = serviceProvider;
            this.appSettings = appSettings;
        }

        public async Task<IEnumerable<GeneralGameSetting>> GetAll()
        {
            var gameEntities = await Task.Run(() => repository.AsQueryable().ToList());

            return gameEntities.Select(game =>
            {
                var gameId = game.GameId > 0 ? game.GameId : (int)Game.FromGameName(game.Name).GameId;
                var displayName = !string.IsNullOrEmpty(game.DisplayName) ? game.DisplayName : Game.FromGameName(game.Name).DisplayName;
                var isEnabledJackpot = game.SlotsConfig?.Jackpot?.Enabled ?? false;

                return new GeneralGameSetting(gameId, game.Name, displayName, (GameType)game.Type, isEnabledJackpot);
            });
        }

        public async Task<string> GetGameName(int gameId)
        {
            if (Enum.IsDefined(typeof(GameId), gameId))
            {
                return Game.FromGameId((GameId)gameId).Value;
            }
            else
            {
                var gameEntities = await Task.Run(() => repository.AsQueryable().ToList());

                var gameInfo = gameEntities.FirstOrDefault(game => game.GameId == gameId);

                return gameInfo?.Name;
            }
        }

        public async Task<int> GetGameId(string name)
        {
            if (Game.ContainsGame(name))
            {
                return (int)Game.FromGameName(name).GameId;
            }
            else
            {
                var gameEntities = await Task.Run(() => repository.AsQueryable().ToList());

                var gameInfo = gameEntities.Find(game => game.Name == name);

                return gameInfo?.GameId ?? default;
            }
        }

        public async Task<byte> GetGameType(int gameId)
        {
            var gameEntities = await Task.Run(() => repository.AsQueryable().ToList());

            var gameInfo = gameEntities.Find(game => game.GameId == gameId);

            return gameInfo?.Type ?? default;
        }

        public async Task<string> GetDisplayName(int gameId)
        {
            var gameInfo = appSettings.CloudSettings?.ShouldCallCloudService(gameId) == true ?
              await GetDisplayNameThroughGameManagementApi(gameId) : await GetDisplayNameDierctOnKbtDatabase(gameId);

            var displayName = !string.IsNullOrEmpty(gameInfo.DisplayName) ? gameInfo.DisplayName : Game.FromGameName(gameInfo.Name).DisplayName;
            return displayName;
        }

        public async Task<bool> IsCrashGame(int gameId)
        {
            return appSettings.CloudSettings.ShouldCallCloudService(gameId) ?
                 await CheckCrashGameThroughGameManagementApi(gameId) :
                 await CheckCrashGameDirectOnKbtDatabase(gameId);
        }

        private async Task<GameConfigEntity> GetDisplayNameDierctOnKbtDatabase(int gameId)
        {
            var gameEntities = await Task.Run(() => repository.AsQueryable().ToList());

            var gameInfo = gameEntities.FirstOrDefault(game => game.GameId == gameId);
            return gameInfo;
        }

        private async Task<GameConfigEntity> GetDisplayNameThroughGameManagementApi(int gameId)
        {
            var gameManagementApi = (IGameManagementApi)serviceProvider.GetService(typeof(IGameManagementApi));
            var response = await gameManagementApi.GetGameConfigById(gameId);
            return response?.Data;
        }

        private async Task<bool> CheckCrashGameDirectOnKbtDatabase(int gameId)
        {
            var gameEntities = await Task.Run(() => repository.AsQueryable().ToList());

            var gameInfo = gameEntities.Find(game => game.GameId == gameId);

            return gameInfo?.Type == (byte)GameType.Crash;
        }

        private async Task<bool> CheckCrashGameThroughGameManagementApi(int gameId)
        {
            var gameManagementApi = (IGameManagementApi)serviceProvider.GetService(typeof(IGameManagementApi));
            var response = await gameManagementApi.GetGameConfigById(gameId);

            if (response?.Data == null || string.IsNullOrWhiteSpace(response?.Data.Name))
            {
                return false;
            }

            return response.Data?.Type == (byte)GameType.Crash;
        }
    }
}