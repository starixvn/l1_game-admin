using System.Linq;
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
            var viewModel = await mediator.Send(new GetOnlineUsersRequest());

            return View("~/Features/Dashboard/Views/OnlineUsers.cshtml", viewModel);
        }

        [HttpGet]
        public async Task<int> RealOnlineUsers()
        {
            return (await mediator.Send(new GetOnlineUsersRequest())).TotalReal;
        }

        [HttpGet]
        public async Task<IActionResult> OnlineUsers()
        {
            var viewModel = await mediator.Send(new GetOnlineUsersRequest());

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetOnlineUsers(bool includeUUS, string game)
        {
            var result = await mediator.Send(new GetOnlineUsersRequest(game));
            var users = result.RealUsers.ToList();

            if (includeUUS)
            {
                users.AddRange(result.UusUsers);
            }

            return PartialView("_OnlineUserTable", users);
        }

        [HttpGet]
        public async Task<IActionResult> BotUsers()
        {
            var viewModel = await mediator.Send(new GetOnlineUsersRequest());

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetBotUsers(string game)
        {
            var result = await mediator.Send(new GetOnlineUsersRequest(game));

            return PartialView("_BotUserTable", result.Bots);
        }
    }
}