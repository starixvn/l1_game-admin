using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class GetTournamentPromotionsByStatusRequest : IRequest<IEnumerable<PromotionConfigViewModel>>
    {
        public GetTournamentPromotionsByStatusRequest(string status)
        {
            Status = status;
        }

        public string Status { get; }
    }
}
