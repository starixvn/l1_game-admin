using System.Collections.Generic;
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
    }

    public interface IGameService
    {
        Task<IList<GameConfig>> GetGames();

        Task<bool> Update(GameConfig game);

        Task<GameConfig> Get(string id);

        Task<bool> Add(GameConfig game);

        Task<bool> Delete(string id);
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
        => gameApi.GetGames();

        public Task<bool> Update(GameConfig game)
        => gameApi.Update(game);

        public Task<bool> Delete(string id)
        => gameApi.Delete(id);

    }
}
