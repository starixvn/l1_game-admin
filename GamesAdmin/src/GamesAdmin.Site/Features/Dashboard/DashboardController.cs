using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.Dashboard.Requests;
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
            return (await mediator.Send(new GetOnlineUsersRequest())).TotalRealPlayer;
        }

        [HttpGet]
        public async Task<IActionResult> OnlineUsers()
        {
            var viewModel = await mediator.Send(new GetOnlineUsersRequest());

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetOnlineUsers(bool includeTestAccount, string game, IEnumerable<string> partnerIds)
        {
            var result = await mediator.Send(new GetOnlineUsersByGameRequest(game));
            var users = result.RealUsers
                .AsEnumerable()
                .Where(user => partnerIds.Any(id => string.Equals(id, user.PartnerId)))
                .ToList();

            if (includeTestAccount)
            {
                users.AddRange(result.TestUsers);
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
            var result = await mediator.Send(new GetOnlineUsersByGameRequest(game));

            return PartialView("_BotUserTable", result.Bots);
        }
    }
}