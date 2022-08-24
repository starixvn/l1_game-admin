using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class GetPromotionByStatusRequest : IRequest<IEnumerable<PromotionModel>>
    {
        public GetPromotionByStatusRequest(string status)
        {
            Status = status;
        }
        public string Status { get; }
    }
}
