using System;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.ExchangeRates.Requests;
using GamesAdmin.Site.Features.ExchangeRates.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.ExchangeRates
{
    [Authorize]
    public class ExchangeRateController : Controller
    {
        private const long offsetValue = 7;

        private readonly IMediator mediator;

        public ExchangeRateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public IActionResult Index()
            => View(new IndexViewModel());

        [HttpGet]
        public async Task<IActionResult> Query(DateTime from, DateTime to)
        {
            var request = new QueryExchangeRateRequest(ToOffset(from), ToOffset(to.AddTicks(TimeSpan.TicksPerDay)));
            var viewModels = await mediator.Send(request);

            return PartialView("_Rates", viewModels);
        }

        private static DateTime ToOffset(DateTime date) => date.AddTicks(-TimeSpan.TicksPerHour * offsetValue);
    }
}