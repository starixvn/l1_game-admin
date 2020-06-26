using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Site.Features.ProVietKeno.Requests
{
    public class NotifyRoundResultRequest : IRequest
    {
        public NotifyRoundResultRequest(IEnumerable<ProVietKenoRound> kenoRounds)
        {
            KenoRounds = kenoRounds;
        }

        public IEnumerable<ProVietKenoRound> KenoRounds { get; }
    }
}