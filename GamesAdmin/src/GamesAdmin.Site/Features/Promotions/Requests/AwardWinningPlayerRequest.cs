using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Core.OWResults;
using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class AwardWinningPlayerRequest : AwardWinningPlayer, IRequest<OWResult>
    {
    }
}
