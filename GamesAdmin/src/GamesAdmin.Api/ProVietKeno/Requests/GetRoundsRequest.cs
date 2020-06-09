using MediatR;
using System.Collections.Generic;
using GamesAdmin.Core.Models;
using System;

namespace GamesAdmin.Api.ProVietKeno.Requests
{
    public class GetRoundsRequest : IRequest<IEnumerable<ProVietKenoRound>>
    {
        public GetRoundsRequest(DateTime date)
        {
            Date = date;
        }

        public DateTime Date { get; }
    }
}