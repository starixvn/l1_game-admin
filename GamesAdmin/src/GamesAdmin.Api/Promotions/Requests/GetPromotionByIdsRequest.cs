using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class GetPromotionByIdsRequest : IRequest<IEnumerable<PromotionModel>>
    {
        public GetPromotionByIdsRequest(string[] ids)
        {
            Ids = ids;
        }
        public string[] Ids { get; }
    }
}
