using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Core.OWResults;
using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class AwardWinningPlayerRequest : AwardWinningPlayer, IRequest<OWResult>
    {
    }
}
