using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }

    public class GeneralGameSettingService : IGeneralGameSettingService
    {
        private readonly IGenericRepository<GameConfigEntity> repository;

        public GeneralGameSettingService(
            IGenericRepository<GameConfigEntity> repository)

        {
            this.repository = repository;
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
            if (Enumeration.Contains<Game>(name))
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
    }
}