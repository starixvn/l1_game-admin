using System.Threading.Tasks;
using GamesAdmin.Site.Features.JackpotSettingsManagement.Requests;
using GamesAdmin.Site.Features.JackpotSettingsManagement.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.JackpotSettingsManagement
{
    [Authorize]
    public class JackpotSettingsController : Controller
    {
        private readonly IMediator mediator;

        public JackpotSettingsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = await mediator.Send(new IndexPageRequest());
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create() => PartialView("_Create", await mediator.Send(new CreatePageRequest()));

        [HttpPost]
        public async Task<IActionResult> UpdatePlayerSettings([Bind] JackpotSettingsViewModel model)
        {
            var result = await mediator.Send(new UpdatePlayerSettingsRequest { Settings = model });
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBotSettings([Bind] JackpotSettingsViewModel model)
        {
            var result = await mediator.Send(new UpdateBotSettingsRequest { Settings = model });
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBotRatioSettings([Bind] JackpotSettingsViewModel model)
        {
            var result = await mediator.Send(new UpdateBotRatioSettingsRequest { Settings = model });
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetJackpotSettingDetails(string configId)
        {
            var result = await mediator.Send(new CopyJackpotSettingsRequest { ConfigId = configId });
            return PartialView("_SettingDetails", result);
        }
    }
}