using System.Threading.Tasks;
using Audit.Mvc;
using AutoMapper;
using GamesAdmin.Site.Features.Chips.Requests;
using GamesAdmin.Site.Features.Chips.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.Chips
{
    [Authorize]
    public class ChipController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public ChipController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
            => View(await mediator.Send(new ChipManagementPageRequest()));

        [HttpGet]
        public IActionResult Create()
            => PartialView("AddChip", new CreateChipViewModel());

        [HttpPost, Audit]
        public async Task<IActionResult> Create([Bind] CreateChipViewModel model)
        {
            var result = await mediator.Send(mapper.Map<CreateChipRequest>(model));

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
             => PartialView("UpdateChip", await mediator.Send(new GetUpdateChipPageRequest(id)));

        [HttpPost, Audit]
        public async Task<IActionResult> Update([Bind] UpdateChipViewModel model)
        {
            var result = await mediator.Send(mapper.Map<UpdateChipRequest>(model));

            return Json(result);
        }
    }
}