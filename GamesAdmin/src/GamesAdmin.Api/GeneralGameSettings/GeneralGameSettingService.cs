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

        Task<GameConfigEntity> GetGameConfig(int gameId);

        Task<IEnumerable<GeneralGameSetting>> GetSlotsGames();
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
                var gameInfo = await GetGameConfigDirectOnKbtDatabase(gameId);

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
                var gameInfo = await Task.Run(() => repository.AsQueryable()
                    .FirstOrDefault(game => game.Name == name));

                return gameInfo?.GameId ?? default;
            }
        }

        public async Task<byte> GetGameType(int gameId)
        {
            var gameInfo = await GetGameConfigDirectOnKbtDatabase(gameId);

            return gameInfo?.Type ?? default;
        }

        public async Task<string> GetDisplayName(int gameId)
        {
            var gameInfo = await GetGameConfig(gameId);

            var displayName = !string.IsNullOrEmpty(gameInfo.DisplayName) ? gameInfo.DisplayName : Game.FromGameName(gameInfo.Name).DisplayName;
            return displayName;
        }

        public async Task<bool> IsCrashGame(int gameId)
        {
            var gameConfig = await GetGameConfig(gameId);
            return gameConfig?.Type == (byte)GameType.Crash || gameConfig?.Type == (byte)GameType.CrashTurnBase;
        }

        public async Task<GameConfigEntity> GetGameConfig(int gameId)
        {
            if (appSettings.CloudSettings?.ShouldCallCloudService(gameId) == true)
            {
                return await GetGameConfigThroughGameManagementApi(gameId);
            }

            return await GetGameConfigDirectOnKbtDatabase(gameId);
        }

        private async Task<GameConfigEntity> GetGameConfigThroughGameManagementApi(int gameId)
        {
            var gameManagementApi = (IGameManagementApi)serviceProvider.GetService(typeof(IGameManagementApi));
            var response = await gameManagementApi.GetGameConfigById(gameId);
            return response?.Data;
        }

        private async Task<GameConfigEntity> GetGameConfigDirectOnKbtDatabase(int gameId)
        {
            return await Task.Run(() => repository.AsQueryable()
                .FirstOrDefault(game => game.GameId == gameId));
        }

        public async Task<IEnumerable<GeneralGameSetting>> GetSlotsGames()
        {
             var gameEntities = (await repository.FilterByAsync(game => game.Type == (byte)GameType.Slots)).ToList();

             return gameEntities.Select(game => new GeneralGameSetting(game.GameId, game.Name, game.DisplayName, (GameType)game.Type, game.SlotsConfig.Jackpot.Enabled));
        }
    }
}