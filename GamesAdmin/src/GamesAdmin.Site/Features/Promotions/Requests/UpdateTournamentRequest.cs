using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class UpdateTournamentRequest : IRequest<MessageResult>
    {
        public UpdateTournamentRequest(PromotionCampaignViewModel model)
        {
            Model = model;
        }

        public PromotionCampaignViewModel Model { get; }
    }
}
