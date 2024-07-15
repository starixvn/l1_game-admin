using GamesAdmin.Core.Models.Promotion;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class GetTournamentPromotionsByStatusRequest : IRequest<IEnumerable<PromotionConfigModel>>
    {
        public GetTournamentPromotionsByStatusRequest(string status)
        {
            Status = status;
        }

        public string Status { get; }
    }
}
