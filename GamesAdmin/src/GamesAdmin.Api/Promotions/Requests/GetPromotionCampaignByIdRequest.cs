using GamesAdmin.Core.Models.Promotion;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class GetPromotionCampaignByIdRequest : IRequest<PromotionConfigModel>
    {
        public GetPromotionCampaignByIdRequest(string id)
        {
            Id = id;
        }
        public string Id { get; }
    }
}
