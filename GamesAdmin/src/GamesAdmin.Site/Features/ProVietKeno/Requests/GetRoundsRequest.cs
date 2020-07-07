using System;
using System.Collections.Generic;
using GamesAdmin.Site.Features.ProVietKeno.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ProVietKeno.Requests
{
    public class GetRoundsRequest : IRequest<IEnumerable<ProVietKenoRoundViewModel>>
    {
        public GetRoundsRequest(DateTime date)
        {
            Date = date;
        }

        public DateTime Date { get; }
    }
}