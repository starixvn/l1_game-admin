using MediatR;
using System.Collections.Generic;
using GamesAdmin.Core.Models;
using System;

namespace GamesAdmin.Api.VietlottKeno.Requests
{
    public class GetRoundsRequest : IRequest<IEnumerable<VietlottKenoRound>>
    {
        public GetRoundsRequest(DateTime date)
        {
            Date = date;
        }

        public DateTime Date { get; }
    }
}