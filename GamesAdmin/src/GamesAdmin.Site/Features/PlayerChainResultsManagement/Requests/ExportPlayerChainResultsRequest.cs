using MediatR;

namespace GamesAdmin.Site.Features.PlayerChainResultsManagement.Requests
{
    public class ExportPlayerChainResultsRequest : IRequest<byte[]>
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }
    }
}
