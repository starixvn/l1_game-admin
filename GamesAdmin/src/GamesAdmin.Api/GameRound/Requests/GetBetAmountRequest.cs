using GamesAdmin.Core.Models;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.GameRound.Requests
{
    public class GetBetAmountRequest : IRequest<IEnumerable<BetInfo>>
    {
    }
}
