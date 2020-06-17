using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.GameSettings.Requests;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sentry;

namespace GamesAdmin.Site.Features.GameSettings
{
    [Authorize]
    public class GameSettingsController : Controller
    {
        private readonly IMediator mediator;
        private readonly ISentryClient sentryClient;

        public GameSettingsController(IMediator mediator, ISentryClient sentryClient)
        {
            this.mediator = mediator;
            this.sentryClient = sentryClient;
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        => View(await mediator.Send(new EditRequest(GameType.BigSmall.Value)));

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> Edit(string name, bool isSuccess = false, string errorMessage = null)
        {
            var viewModel = await mediator.Send(new EditRequest(name));
            viewModel.ErrorMessage = TempData["errorMessage"]?.ToString();
            viewModel.Success = TempData["isSuccess"] == null ? false : (bool)TempData["isSuccess"];

            return View(viewModel);
        }

        [HttpPost]
        [Route("/update")]
        public async Task<IActionResult> Update([Bind] EditViewModel editViewModel)
        {
            try
            {
                await mediator.Send(new UpdateRequest(new GameConfig(
                   editViewModel.Id,
                   editViewModel.Name,
                   editViewModel.MinBet,
                   editViewModel.MaxBet,
                   editViewModel.Enabled,
                   editViewModel.BotEnabled,
                   editViewModel.MaxBot,
                   null,
                   0,
                   editViewModel.BotMaxBet,
                   null,
                   editViewModel.DisabledMessage,
                   editViewModel.MaxBetChoices,
                   editViewModel.Odds)));

                return Json(true);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return Json(false);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        => View(await mediator.Send(new AddRequest()));

        [HttpPost]
        public async Task<IActionResult> Add([Bind] AddViewModel addViewModel)
        {
            await mediator.Send(new CreateRequest(new GameConfig(
                string.Empty,
                addViewModel.Name,
                addViewModel.MinBet,
                addViewModel.MaxBet,
                addViewModel.Enabled,
                addViewModel.BotEnabled,
                addViewModel.MaxBot,
                null,
                0,
                0,
                new double[] { },
                string.Empty,
                new Dictionary<string, double>(),
                new Dictionary<string, double>())));

            return RedirectToAction(nameof(Edit));
        }

        [HttpPost]
        [Route("{name}")]
        public async Task<IActionResult> UpdateBotRatio(string name, [Bind] List<BotRatioItemViewModel> botRatioItems)
        {
            var botRatios = botRatioItems
                .OrderBy(item => item.UtcHour)
                .Select(item => item.BotRatio / 100)
                .ToArray();

            await mediator.Send(new UpdateBotRatioRequest(name, botRatios));

            TempData["isSuccess"] = true;

            return Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> GameSettings()
            => View("~/Features/GameSettings/Views/GameManagement.cshtml", await mediator.Send(new GetStatusRequest()));

        [HttpGet]
        public async Task<IActionResult> Index()
            => View("~/Features/GameSettings/Views/GameMonitor.cshtml", await mediator.Send(new GetStatusRequest()));


        [HttpGet]
        [Route("gamedetails/{name}")]
        public async Task<IActionResult> GameDetails(string name)
        {
            var viewModel = await mediator.Send(new EditRequest(name));
            viewModel.ErrorMessage = TempData["errorMessage"]?.ToString();
            viewModel.Success = TempData["isSuccess"] == null ? false : (bool)TempData["isSuccess"];

            return View(viewModel);
        }

        [HttpGet]
        [Route("botratio/{gameName}")]
        public async Task<IActionResult> BotRatio(string gameName)
        {
            var gameSetting = await mediator.Send(new EditRequest(gameName));

            return Json(gameSetting);
        }

        [HttpPost]
        [Route("status/enable")]
        public async Task<IActionResult> EnableGames(string[] games, bool reload = false)
        {
            var result = await mediator.Send(new UpdateMultipleGameStatusRequest(games, true, reload));

            return Json(result);
        }

        [HttpPost]
        [Route("status/disable")]
        public async Task<IActionResult> DisableGames(string[] games, bool reload = false)
        {
            var result = await mediator.Send(new UpdateMultipleGameStatusRequest(games, false, reload));

            return Json(result);
        }

        [HttpPost]
        [Route("status")]
        public async Task<IActionResult> UpdateDisabledMessage(string name, string disabledMessage)
        {
            if (string.IsNullOrWhiteSpace(disabledMessage))
            {
                TempData["errorMessage"] = "Message cannot be empty.";
                return Json(false);
            }
            else
            {
                await mediator.Send(new UpdateDisabledMessageRequest(name, disabledMessage));
                return Json(true);
            }
        }

        [HttpPost]
        [Route("status/update")]
        public async Task<IActionResult> UpdateAllDisabledMessage(string disabledMessage)
        {
            if (string.IsNullOrWhiteSpace(disabledMessage))
            {
                TempData["errorMessage"] = "Message cannot be empty.";
                return Json(false);
            }
            else
            {
                await mediator.Send(new UpdateAllDisabledMessageRequest(disabledMessage));
                return Json(true);
            }
        }

        [HttpPost]
        [Route("status/reload")]
        public async Task<IActionResult> ReloadGameClients()
        {
            var result = await mediator.Send(new ReloadGameClientRequest());

            return Json(result);
        }

        [HttpPost]
        [Route("/jwt")]
        public async Task<IActionResult> JWT([FromBody] GetJWTRequest request)
        {
            request.Username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;

            var result = await mediator.Send(request);

            HttpContext.Response.Cookies.Append("token", result.Token);
            HttpContext.Response.Cookies.Append("game-server-endpoint", result.GameServerEndpoint);

            return Json(result);
        }
    }
}