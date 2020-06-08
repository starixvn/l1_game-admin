using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.VietlottKeno.Requests;
using GamesAdmin.Site.Features.VietlottKeno.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.VietlottKeno
{
    [Authorize]
    public class VietlottKenoController : Controller
    {
        private readonly IMediator mediator;

        public VietlottKenoController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = new VietlottKenoViewModel();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetVietlottKenoRounds(DateTime date)
        {
            var result = await mediator.Send(new GetRoundsRequest(date.ToUniversalTime()));
            return PartialView("_VietlottKenoRounds", result);
        }

        [HttpGet]
        public IActionResult AddNewRound()
        {
            var viewModel = new CreateVietlottKenoRoundViewModel();
            return PartialView("_AddNewRound", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewRound(CreateVietlottKenoRoundViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Result.Count(x => x) == 20)
                {
                    await mediator.Send(new CreateRoundRequest(viewModel));
                    return Json(new { isSuccessful = true });
                }
                else
                {
                    ModelState.AddModelError(nameof(CreateVietlottKenoRoundViewModel.Result), "Please select 20 numbers");
                }
            }

            var errors = ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors.First().ErrorMessage });
            return Json(new { isSuccessful = false, errors });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> NotifyRoundResult([FromBody] IEnumerable<VietlottKenoRound> kenoRounds)
        {
            await mediator.Send(new NotifyRoundResultRequest(kenoRounds));
            return Ok();
        }
    }
}