using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.GameSettings.Requests;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.GameSettings
{
    [Authorize]
    public class GameSettingsController : Controller
    {
        private readonly IMediator mediator;

        public GameSettingsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        => View(await mediator.Send(new EditRequest(string.Empty)));

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> Edit(string name)
        => View(await mediator.Send(new EditRequest(name)));

        [HttpPost]
        [Route("/update")]
        public async Task<IActionResult> Update([Bind] EditViewModel editViewModel)
        {
            try
            {
                var updated = await mediator.Send(new UpdateRequest(new GameConfig(
                   editViewModel.Id,
                   editViewModel.Name,
                   editViewModel.MinBet,
                   editViewModel.MaxBet,
                   editViewModel.Enabled,
                   editViewModel.BotEnabled,
                   editViewModel.MaxBot,
                   null,
                   editViewModel.BotMinBet,
                   editViewModel.BotMaxBet)));

                //editViewModel.Success = updated;

                //if (!editViewModel.Success)
                //{
                //    editViewModel.ErrorMessage = "Something went wrong. Cannot update this config.";
                //}

                return RedirectToAction(nameof(Edit), new RouteValueDictionary { { "name", editViewModel.Name } });
            }
            catch (Exception ex) 
            {
                editViewModel.Success = false;
                editViewModel.ErrorMessage = ex.Message;
            }

            return View(editViewModel);
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
                0)));

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
    }
}