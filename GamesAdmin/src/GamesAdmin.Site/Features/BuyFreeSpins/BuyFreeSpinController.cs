using System.Threading.Tasks;
using Audit.Mvc;
using AutoMapper;
using GamesAdmin.Site.Features.BuyFreeSpins.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.BuyFreeSpins
{
    [Authorize]
    public class BuyFreeSpinController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public BuyFreeSpinController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
            => View(await mediator.Send(new GetAllBuyFreeSpinsRequest()));

        [HttpGet]
        public async Task<IActionResult> Create(){
            return PartialView("AddBuyFreeSpin", await mediator.Send(new GetCreateBuyFreeSpinPageRequest()));
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind] CreateBuyFreeSpinRequest model)
        {
            var result = await mediator.Send(model);

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int gameId)
             => PartialView("UpdateBuyFreeSpin", await mediator.Send(new GetUpdateBuyFreeSpinPageRequest(gameId)));

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([Bind] UpdateBuyFreeSpinRequest model)
        {
            var result = await mediator.Send(model);

            return Json(result);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Enable([Bind] EnableBuyFreeSpinRequest model)
        {
            var result = await mediator.Send(model);

            return Json(result);
        }
    }
}