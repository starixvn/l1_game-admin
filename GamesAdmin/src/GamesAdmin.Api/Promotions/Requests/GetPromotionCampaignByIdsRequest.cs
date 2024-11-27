using GamesAdmin.Core.Models.Promotion;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class GetPromotionCampaignByIdsRequest : IRequest<IEnumerable<PromotionConfigModel>>
    {
        public GetPromotionCampaignByIdsRequest(string[] ids)
        {
            Ids = ids;
        }
        public string[] Ids { get; }
    }
}
