using MediatR;

namespace GamesAdmin.Site.Features.Partners.Requests
{
    public class GetSiteExclusionRequest : IRequest<string[]>
    {
        public GetSiteExclusionRequest(string partnerId, int gameId)
        {
            PartnerId = partnerId;
            GameId = gameId;
        }

        public string PartnerId { get; }

        public int GameId { get; set; }
    }
}