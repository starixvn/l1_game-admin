using GamesAdmin.Core.Models;
using GamesAdmin.Site._Shared.Enumerations;
using GamesAdmin.Site.Features.GameSettings.Requests;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                   editViewModel.BotMinBet,
                   editViewModel.BotMaxBet,
                   null,
                   string.Empty)));

                TempData["isSuccess"] = true;

                return RedirectToAction(nameof(Edit), new RouteValueDictionary
                {
                    { "name", editViewModel.Name }
                });
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                sentryClient.CaptureException(ex);
            }

            return RedirectToAction(nameof(Edit), new RouteValueDictionary
            {
                { "name", editViewModel.Name }
            });
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
                string.Empty)));

            return RedirectToAction(nameof(Edit));
        }

        [HttpGet]
        [Route("{id}/Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            await mediator.Send(new DeleteRequest(id));

            return RedirectToAction(nameof(Edit));
        }

        [AcceptVerbs("GET")]
        public async Task<IActionResult> VerifyName(string name)
        {
            var isExist = await mediator.Send(new IsExistRequest(name));

            return isExist ? Json($"A config named {name} already exists.") : Json(true);
        }

        [HttpPost]
        [Route("{name}")]
        public async Task<IActionResult> UpdateBotRatio(string name, [Bind]List<BotRatioItemViewModel> botRatioItems)
        {
            var botRatios = botRatioItems
                .OrderBy(item => item.UtcHour)
                .Select(item => item.BotRatio / 100)
                .ToArray();

            await mediator.Send(new UpdateBotRatioRequest(name, botRatios));

            TempData["isSuccess"] = true;

            return RedirectToAction(nameof(Edit), new RouteValueDictionary
            {
                { "name", name }
            });
        }

        [HttpGet]
        [Route("status")]
        public async Task<IActionResult> Status()
        => View(await mediator.Send(new GetStatusRequest()));

        [HttpPost]
        [Route("status/enable")]
        public async Task<IActionResult> EnableGames(string[] games)
        {
            var result = await mediator.Send(new UpdateMultipleGameStatusRequest(games, true));

            return Json(result);
        }

        [HttpPost]
        [Route("status/disable")]
        public async Task<IActionResult> DisableGames(string[] games)
        {
            var result = await mediator.Send(new UpdateMultipleGameStatusRequest(games, false));

            return Json(result);
        }

        [HttpPost]
        [Route("status")]
        public async Task<IActionResult> UpdateDisabledMessage(string name, string disabledMessage)
        {
            await mediator.Send(new UpdateDisabledMessageRequest(name, disabledMessage));

            return RedirectToAction(nameof(Status));
        }

        [HttpPost]
        [Route("status/update")]
        public async Task<IActionResult> UpdateAllDisabledMessage(string disabledMessage)
        {
            await mediator.Send(new UpdateAllDisabledMessageRequest(disabledMessage));

            return RedirectToAction(nameof(Status));
        }
    }
}