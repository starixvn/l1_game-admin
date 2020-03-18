using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using Refit;

namespace GamesAdmin.Site.Features.GameSettings
{
    public interface IGameApi
    {
        [Get("/games")]
        Task<IList<GameConfig>> GetGames();

        [Get("/games/{id}")]
        Task<GameConfig> GetGame(string id);

        [Post("/games/")]
        Task<bool> Add([Body] GameConfig game);

        [Put("/games/")]
        Task<bool> Update([Body] GameConfig game);

        [Post("/games/{id}/delete")]
        Task<bool> Delete(string id);

        [Get("/games/{name}")]
        Task<GameConfig> GetByName(string name);
    }

    public interface IGameService
    {
        Task<IList<GameConfig>> GetGames();

        Task<bool> Update(GameConfig game);

        Task<GameConfig> Get(string id);

        Task<bool> Add(GameConfig game);

        Task<bool> Delete(string id);

        Task<GameConfig> GetByName(string name);

        Task<bool> IsExistName(string name);
    }

    public class GameService : IGameService
    {
        private readonly IGameApi gameApi;

        public GameService(IGameApi gameApi)
        {
            this.gameApi = gameApi;
        }

        public Task<bool> Add(GameConfig game)
        => gameApi.Add(game);

        public Task<GameConfig> Get(string id)
        => gameApi.GetGame(id);

        public Task<IList<GameConfig>> GetGames()
        {
            try
            {
                return gameApi.GetGames();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;

                return Task.FromResult(new List<GameConfig>() as IList<GameConfig>);
            }
        }

        public Task<bool> Update(GameConfig game)
        => gameApi.Update(game);

        public Task<bool> Delete(string id)
        => gameApi.Delete(id);

        public Task<GameConfig> GetByName(string name)
        => gameApi.GetByName(name);

        public async Task<bool> IsExistName(string name)
        {
            var gameConfig = await GetByName(name);

            return gameConfig != default;
        }
    }
}
