using GamesAdmin.Site.Features.Partners.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Partners.Requests
{
    public class UpdateSiteExclusionRequest : IRequest<bool>
    {
        public UpdateSiteExclusionRequest(string partnerId, int gameId, string[] exclusionSites)
        {
            PartnerId = partnerId;
            GameId = gameId;
            ExclusionSites = exclusionSites;
        }

        public string PartnerId { get; }

        public int GameId { get; set; }

        public string[] ExclusionSites { get; set; }
    }
}
