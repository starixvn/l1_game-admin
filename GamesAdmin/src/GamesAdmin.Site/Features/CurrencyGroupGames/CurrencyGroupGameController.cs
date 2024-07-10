using System.Linq;
using System.Threading.Tasks;
using Audit.Mvc;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
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
        public async Task<IActionResult> Index(string partnerId, string partnerName, string gameName, GameType gameType)
        {
            var viewModel = await mediator.Send(new IndexRequest(partnerId, partnerName));
            viewModel.GameName = gameName;
            viewModel.GameType = gameType;

            return PartialView("_Index", viewModel);
        }

        [HttpGet]
        [Route("currencyGroupGame/Edit")]
        public async Task<IActionResult> Edit(string gameName, string groupId, bool isCopySetting, GameType gameType)
        {
            var viewModel = await mediator.Send(new GetOneRequest(groupId, gameName, isCopySetting));
            viewModel.GameType = gameType;

            return PartialView("_Edit", viewModel);
        }

        [HttpPost, Audit]
        [Route("currencyGroupGame/Edit")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(EditViewModel viewModel)
        {
            var result = await mediator.Send(new EditRequest(mapper.Map<CurrencyGroupGame>(viewModel)));

            return Json(result);
        }
    }
}