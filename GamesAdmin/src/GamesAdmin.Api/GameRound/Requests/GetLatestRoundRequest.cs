using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameRound.Requests
{
    public class GetLatestRoundRequest : IRequest<Round>
    {
    }
}
