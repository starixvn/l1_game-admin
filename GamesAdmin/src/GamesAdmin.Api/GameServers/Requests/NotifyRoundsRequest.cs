using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class NotifyRoundsRequest : IRequest<bool>
    {
        public NotifyRoundsRequest(IEnumerable<ProVietKenoRound> rounds)
        {
            Rounds = rounds;
        }

        public IEnumerable<ProVietKenoRound> Rounds { get; }
    }
}
