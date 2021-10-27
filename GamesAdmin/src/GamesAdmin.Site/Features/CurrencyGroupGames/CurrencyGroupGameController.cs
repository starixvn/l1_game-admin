using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.CurrencyGroupGames.Requests;
using GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.CurrencyGroupGames
{
    [Authorize]
    public class CurrencyGroupGameController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public CurrencyGroupGameController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string partnerId, string partnerName, int gameId)
        {
            var viewModel = await mediator.Send(new IndexRequest(partnerId, partnerName));
            viewModel.GameId = gameId;

            return PartialView("_Index", viewModel);
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int gameId, string groupId)
        {
            var viewModel = await mediator.Send(new GetOneRequest(groupId, gameId));

            return PartialView("_Edit", viewModel);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(EditViewModel viewModel)
        {
            var result = await mediator.Send(new EditRequest(mapper.Map<CurrencyGroupGame>(viewModel)));

            return Json(result);
        }
    }
}
