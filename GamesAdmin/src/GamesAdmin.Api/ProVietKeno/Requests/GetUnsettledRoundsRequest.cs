using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.ProVietKeno.Requests
{
    public class GetUnsettledRoundsRequest : IRequest<IEnumerable<ProVietKenoRound>>
    {
    }
}