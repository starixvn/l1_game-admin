using GamesAdmin.Core.Models.Promotion;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class GetPromotionCampaignByStatusRequest : IRequest<IEnumerable<PromotionConfigModel>>
    {
        public GetPromotionCampaignByStatusRequest(string status)
        {
            Status = status;
        }
        public string Status { get; }
    }
}
