using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var viewModel = new AddVietlottKenoRoundViewModel();
            return PartialView("_AddNewRound", viewModel);
        }
    }
}