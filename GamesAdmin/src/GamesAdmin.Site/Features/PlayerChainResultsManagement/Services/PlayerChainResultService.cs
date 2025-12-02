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
        Task<PlayerChainResultsResponse> GetPlayerChainResults(PlayerChainResultsQuery query);
        
        [Get("/player-chain-results/export")]
        Task<HttpResponseMessage> ExportPlayerChainResults(PlayerChainResultsQuery query);
    }

    public interface IPlayerChainResultService
    {
        Task<PlayerChainResultsResponse> GetPlayerChainResults(PlayerChainResultsQuery query);
        Task<byte[]> ExportPlayerChainResults(PlayerChainResultsQuery query);
    }

    public class PlayerChainResultService : IPlayerChainResultService
    {
        private readonly IPlayerChainResultApi playerChainResultApi;

        public PlayerChainResultService(IPlayerChainResultApi playerChainResultApi)
        {
            this.playerChainResultApi = playerChainResultApi;
        }

        public Task<PlayerChainResultsResponse> GetPlayerChainResults(PlayerChainResultsQuery query)
        {
            return playerChainResultApi.GetPlayerChainResults(query);
        }

        public async Task<byte[]> ExportPlayerChainResults(PlayerChainResultsQuery query)
        {
            var response = await playerChainResultApi.ExportPlayerChainResults(query);
            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
