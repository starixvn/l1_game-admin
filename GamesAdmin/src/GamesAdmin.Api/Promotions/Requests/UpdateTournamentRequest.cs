using GamesAdmin.Core.Models.Promotion;
using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class UpdateTournamentRequest : IRequest<bool>
    {
        public UpdateTournamentRequest(PromotionConfigModel model, string type)
        {
            Model = model;
            Type = type;
        }
        public string Type { get; }
        public PromotionConfigModel Model { get; }
    }
}
