using System;
using GamesAdmin.Site.Features.PlayerChainResultsManagement.ViewModels;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.PlayerChainResultsManagement.Requests
{
    public class GetPlayerChainResultsRequest : IRequest<PlayerChainResultsResponse>
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool ExcludeUUS { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }
}
