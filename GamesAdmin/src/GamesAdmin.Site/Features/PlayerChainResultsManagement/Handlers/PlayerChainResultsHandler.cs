using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.PlayerChainResultsManagement.Requests;
using GamesAdmin.Site.Features.PlayerChainResultsManagement.Services;
using GamesAdmin.Site.Features.PlayerChainResultsManagement.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.PlayerChainResultsManagement.Handlers
{
    public class PlayerChainResultsHandler : IRequestHandler<GetPlayerChainResultsRequest, PlayerChainResultsResponse>
    {
        private readonly IPlayerChainResultService playerChainResultService;

        public PlayerChainResultsHandler(IPlayerChainResultService playerChainResultService)
        {
            this.playerChainResultService = playerChainResultService;
        }

        public async Task<PlayerChainResultsResponse> Handle(GetPlayerChainResultsRequest request, CancellationToken cancellationToken)
        {
            return await playerChainResultService.GetPlayerChainResults(
                request.GameId, 
                request.PlayerId, 
                request.Page, 
                request.PageSize);
        }
    }
}
