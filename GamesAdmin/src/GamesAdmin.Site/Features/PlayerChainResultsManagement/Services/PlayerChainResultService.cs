using System.Collections.Generic;
using System.Net.Http;
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
        
        [Get("/player-chain-results/export")]
        Task<HttpResponseMessage> ExportPlayerChainResults(string gameId = null, string playerId = null);
    }

    public interface IPlayerChainResultService
    {
        Task<PlayerChainResultsResponse> GetPlayerChainResults(string gameId = null, string playerId = null, int page = 1, int pageSize = 25);
        Task<byte[]> ExportPlayerChainResults(string gameId = null, string playerId = null);
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

        public async Task<byte[]> ExportPlayerChainResults(string gameId = null, string playerId = null)
        {
            var response = await playerChainResultApi.ExportPlayerChainResults(gameId, playerId);
            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
