using MediatR;

namespace GamesAdmin.Api.Partners.Requests
{
    public class UpdateSiteExclusionRequest : IRequest<bool>
    {
        public UpdateSiteExclusionRequest() { }

        public UpdateSiteExclusionRequest(string partnerId, int gameId, string[] exclusionSites)
        {
            PartnerId = partnerId;
            GameId = gameId;
            ExclusionSites = exclusionSites;
        }

        public string PartnerId { get; set; }

        public int GameId { get; set; }

        public string[] ExclusionSites { get; set; }
    }
}