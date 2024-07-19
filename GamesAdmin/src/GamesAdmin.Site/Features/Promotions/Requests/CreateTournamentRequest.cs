using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class CreateTournamentRequest : IRequest<MessageResult>
    {
        public CreateTournamentRequest(PromotionCampaignViewModel model)
        {
            Model = model;
        }

        public PromotionCampaignViewModel Model { get; }
    }
}
