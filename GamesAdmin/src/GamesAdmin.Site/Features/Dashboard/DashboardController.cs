using System.Threading.Tasks;
using GamesAdmin.Site.Features.Dashboard.Requests;
using GamesAdmin.Site.Features.Dashboard.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.Dashboard
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IMediator mediator;

        public DashboardController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                OnlineUsers = await mediator.Send(new GetOnlineUsersRequests()),
                TodayTotalBets = 0 // await mediator.Send(new GetTodayTotalBetsRequest())
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<int> OnlineUsers()
        {
            return await mediator.Send(new GetOnlineUsersRequests());
        }

        [HttpGet]
        public async Task<long> TodayTotalBets()
        {
            return await mediator.Send(new GetTodayTotalBetsRequest());
        }
    }
}
