using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class GetTournamentByIdRequest : IRequest<PromotionCampaignViewModel>
    {
        public GetTournamentByIdRequest(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
