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
