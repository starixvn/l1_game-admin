using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using Refit;

namespace GamesAdmin.Site.Features.GameSettings
{
    public interface IGameApi
    {
        [Get("/games")]
        Task<IList<Game>> GetGames();

        [Get("/games/{id}")]
        Task<Game> GetGame(string id);

        [Post("/games/")]
        Task<bool> Add([Body] Game game);

        [Put("/games/{id}")]
        Task<bool> Update(string id, [Body] Game game);
    }

    public interface IGameService
    {
        Task<IList<Game>> GetGames();

        Task<bool> Update(string id, Game game);

        Task<Game> Get(string id);
    }

    public class GameService : IGameService
    {
        private readonly IGameApi gameApi;

        public GameService(IGameApi gameApi)
        {
            this.gameApi = gameApi;
        }

        public Task<Game> Get(string id)
        => gameApi.GetGame(id);

        public Task<IList<Game>> GetGames()
        => gameApi.GetGames();

        public Task<bool> Update(string id, Game game)
        => gameApi.Update(id, game);
    }
}
