using System.Collections.Generic;
using MediatR;
using GamesAdmin.Api.PlayerChainResults.ViewModels;

namespace GamesAdmin.Api.PlayerChainResults.Requests
{
    public class GetPlayerChainResultsRequest : IRequest<ViewModels.PlayerChainResultsResponse>
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }
}
