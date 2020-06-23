using MediatR;
using System.Collections.Generic;
using GamesAdmin.Core.Models;
using System;

namespace GamesAdmin.Api.ProVietKeno.Requests
{
    public class GetRoundsRequest : IRequest<IEnumerable<ProVietKenoRound>>
    {
        public GetRoundsRequest(DateTimeOffset date)
        {
            Date = date;
        }

        public DateTimeOffset Date { get; }
    }
}