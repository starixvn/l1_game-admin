using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class GetPromotionByGameIdRequest : IRequest<IEnumerable<PromotionModel>>
    {
        public GetPromotionByGameIdRequest(GameId gameId)
        {
            GameId = gameId;
        }
        public GameId GameId { get; }
    }
}
