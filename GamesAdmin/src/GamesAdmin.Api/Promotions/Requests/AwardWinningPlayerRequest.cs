using GamesAdmin.Core.Models.Promotion;
using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class AwardWinningPlayerRequest : AwardWinningPlayer, IRequest<AwardWinningPlayer>
    {
    }
}