using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameRound.Requests
{
    public class GetBetAmountRequest : IRequest<Round>
    {
    }
}
