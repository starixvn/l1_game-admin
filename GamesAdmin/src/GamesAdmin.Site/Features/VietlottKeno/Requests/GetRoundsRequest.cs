using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.VietlottKeno.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.VietlottKeno.Requests
{
    public class GetRoundsRequest : IRequest<IEnumerable<ViewlottKenoRoundViewModel>>
    {
        public GetRoundsRequest(DateTime date)
        {
            Date = date;
        }

        public DateTime Date { get; }
    }
}