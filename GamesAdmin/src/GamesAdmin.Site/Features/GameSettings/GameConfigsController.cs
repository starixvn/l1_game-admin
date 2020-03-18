using System;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.GameSettings.Requests;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.GameSettings
{
    [Authorize]
    public class GameConfigsController : Controller
    {
        private readonly IMediator mediator;

        public GameConfigsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index()
        => View(await mediator.Send(new IndexRequest()));

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Edit(string id)
        => View(await mediator.Send(new EditRequest(id)));

        [HttpPost]
        [Route("{id}/update")]
        public async Task<IActionResult> Update([Bind] EditViewModel editViewModel)
        {
            await mediator.Send(new UpdateRequest(new GameConfig(
                editViewModel.Id,
                editViewModel.Name,
                editViewModel.MinBet,
                editViewModel.MaxBet,
                editViewModel.Enabled,
                null)));

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        => View(await mediator.Send(new AddRequest()));

        [HttpPost]
        public async Task<IActionResult> Add([Bind] AddViewModel editViewModel)
        {
            await mediator.Send(new CreateRequest(new GameConfig(
                Guid.Empty,
                editViewModel.Name,
                editViewModel.MinBet,
                editViewModel.MaxBet,
                editViewModel.Enabled,
                null)));

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("{id}/Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            await mediator.Send(new DeleteRequest(id));

            return RedirectToAction(nameof(Index));
        }

        [AcceptVerbs("GET")]
        public async Task<IActionResult> VerifyName(string name)
        {
            var isExist = await mediator.Send(new IsExistRequest(name));

            return isExist ? Json($"A config named {name} already exists.") : Json(true);
        }
    }
}