using System.Collections.Generic;
using System.Threading.Tasks;
using L1.games.admin.core.Models;
using Refit;

namespace L1.games.admin.site.Features.GameSettings
{
    public interface IGameApi 
    {
        [Get("/games")]
        Task<IList<Game>> GetGames();
    }
    
    public interface IGameService
    {
        Task<IList<Game>> GetGames();
    }

    public class GameService : IGameService
    {
        private readonly IGameApi gameApi;

        public GameService(IGameApi gameApi) 
        {
            this.gameApi = gameApi;
        }

        public async Task<IList<Game>> GetGames()
        {
            var games = await gameApi.GetGames();

            return games;
        }
    }
}
