using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Site.Features.VietlottKeno.Requests
{
    public class NotifyRoundResultRequest : IRequest
    {
        public NotifyRoundResultRequest(IEnumerable<VietlottKenoRound> kenoRounds)
        {
            KenoRounds = kenoRounds;
        }

        public IEnumerable<VietlottKenoRound> KenoRounds { get; }
    }
}