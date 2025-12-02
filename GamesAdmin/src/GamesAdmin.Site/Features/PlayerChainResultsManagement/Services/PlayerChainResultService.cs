using System;
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
        Task<PlayerChainResultsResponse> GetPlayerChainResults(string gameId = null, string playerId = null, DateTime? fromDate = null, DateTime? toDate = null, bool excludeUUS = false, int page = 1, int pageSize = 25);
        
        [Get("/player-chain-results/export")]
        Task<HttpResponseMessage> ExportPlayerChainResults(string gameId = null, string playerId = null, DateTime? fromDate = null, DateTime? toDate = null, bool excludeUUS = false);
    }

    public interface IPlayerChainResultService
    {
        Task<PlayerChainResultsResponse> GetPlayerChainResults(string gameId = null, string playerId = null, DateTime? fromDate = null, DateTime? toDate = null, bool excludeUUS = false, int page = 1, int pageSize = 25);
        Task<byte[]> ExportPlayerChainResults(string gameId = null, string playerId = null, DateTime? fromDate = null, DateTime? toDate = null, bool excludeUUS = false);
    }

    public class PlayerChainResultService : IPlayerChainResultService
    {
        private readonly IPlayerChainResultApi playerChainResultApi;

        public PlayerChainResultService(IPlayerChainResultApi playerChainResultApi)
        {
            this.playerChainResultApi = playerChainResultApi;
        }

        public Task<PlayerChainResultsResponse> GetPlayerChainResults(string gameId = null, string playerId = null, DateTime? fromDate = null, DateTime? toDate = null, bool excludeUUS = false, int page = 1, int pageSize = 25)
        {
            return playerChainResultApi.GetPlayerChainResults(gameId, playerId, fromDate, toDate, excludeUUS, page, pageSize);
        }

        public async Task<byte[]> ExportPlayerChainResults(string gameId = null, string playerId = null, DateTime? fromDate = null, DateTime? toDate = null, bool excludeUUS = false)
        {
            var response = await playerChainResultApi.ExportPlayerChainResults(gameId, playerId, fromDate, toDate, excludeUUS);
            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
