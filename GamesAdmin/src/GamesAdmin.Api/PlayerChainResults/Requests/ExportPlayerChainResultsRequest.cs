using System;
using MediatR;

namespace GamesAdmin.Api.PlayerChainResults.Requests
{
    public class ExportPlayerChainResultsRequest : IRequest<byte[]>
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool ExcludeUUS { get; set; }
    }
}
