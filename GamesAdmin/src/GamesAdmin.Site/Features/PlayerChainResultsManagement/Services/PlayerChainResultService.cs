using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.PlayerChainResultsManagement.ViewModels;
using Refit;

namespace GamesAdmin.Site.Features.PlayerChainResultsManagement.Services
{
    public interface IPlayerChainResultApi : IBaseAuthorizationApi
    {
        [Get("/player-chain-results")]
        Task<PlayerChainResultsResponse> GetPlayerChainResults(string gameId = null, string playerId = null, int page = 1, int pageSize = 25);
    }

    public interface IPlayerChainResultService
    {
        Task<PlayerChainResultsResponse> GetPlayerChainResults(string gameId = null, string playerId = null, int page = 1, int pageSize = 25);
    }

    public class PlayerChainResultService : IPlayerChainResultService
    {
        private readonly IPlayerChainResultApi playerChainResultApi;

        public PlayerChainResultService(IPlayerChainResultApi playerChainResultApi)
        {
            this.playerChainResultApi = playerChainResultApi;
        }

        public Task<PlayerChainResultsResponse> GetPlayerChainResults(string gameId = null, string playerId = null, int page = 1, int pageSize = 25)
        {
            return playerChainResultApi.GetPlayerChainResults(gameId, playerId, page, pageSize);
        }
    }
}
