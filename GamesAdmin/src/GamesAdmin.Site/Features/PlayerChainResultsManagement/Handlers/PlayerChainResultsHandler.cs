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
            var query = new PlayerChainResultsQuery
            {
                GameId = request.GameId,
                PlayerId = request.PlayerId,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                ExcludeUUS = request.ExcludeUUS,
                Page = request.Page,
                PageSize = request.PageSize
            };

            return await playerChainResultService.GetPlayerChainResults(query);
        }
    }
}
