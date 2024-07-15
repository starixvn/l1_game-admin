using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Audit.Mvc;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.GameSettings.Requests;
using GamesAdmin.Site.Features.Promotions.Requests;
using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Promotions
{
    [Authorize]
    public class PromotionController : Controller
    {
        private readonly IMediator mediator;

        public PromotionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel();
            return View(viewModel);
        }

        public IActionResult Tournament()
        {
            var viewModel = new TournamentViewModel();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetPromotions(string status)
        => PartialView("_Promotions", await mediator.Send(new GetPromotionByStatusRequest(status)));

        [HttpGet]
        public async Task<IActionResult> GetTournamentPromotions(string status)
        => PartialView("_TournamentPromotions", await mediator.Send(new GetTournamentPromotionsByStatusRequest(status)));

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreatePromotionViewModel();
            viewModel.GameList = (await mediator.Send(new GetGamesIdByTypeRequest(GameType.AutoRNG)))
                .Select(game => new SelectListItem(game.DisplayName, $"{(int)game.GameId}"));
            return PartialView("_AddPromotion", viewModel);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind] CreatePromotionViewModel model)
        {
            var result = await mediator.Send(new CreatePromotionRequest(model));

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetOddsByGame(GameId gameId)
        {
            var odds = (await mediator.Send(new GetOddsByGameRequest(gameId))).ToList();
            var viewModel = new PromotionViewModel();
            viewModel.ListOdds = odds;
            return PartialView("_Odds", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var viewModel = await mediator.Send(new GetPromotionByIdRequest(id));
            viewModel.ListOdds = (await mediator.Send(new GetOddsByGameRequest(viewModel.GameId))).ToList();
            viewModel.OriginalOdds = (await mediator.Send(new GetOddsByGameRequest(viewModel.GameId))).ToList();
            foreach (var odds in viewModel.ListOdds)
            {
                odds.Odds = viewModel.Odds[odds.Name];
            }
            return PartialView("_UpdatePromotion", viewModel);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([Bind] PromotionViewModel model)
        {
            var result = await mediator.Send(new UpdatePromotionRequest(model));

            return Json(result);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await mediator.Send(new DeletePromotionRequest(id));

            return Json(result);
        }

        public IActionResult Announcement()
        => View("Announcement", new IndexViewModel());

        public async Task<IActionResult> GetAnnouncementByStatus(string status)
        => PartialView("_AnnouncementTable", await mediator.Send(new GetAnnouncementByStatusRequest(status)));

        [HttpGet]
        public async Task<IActionResult> CreateAnnouncement()
        {
            var viewModel = new CreateAnnouncementItemViewModel();
            viewModel.Promotions = (await mediator.Send(new GetPromotionByStatusRequest(PromotionStatus.Enabled.Value)))
                .Where(promotion => !promotion.HasAnnouncement);
            return PartialView("_AnnouncementModalItem", viewModel);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateAnnouncement([FromBody] IEnumerable<CreateAnnouncementItemViewModel> models)
        {
            var result = await mediator.Send(new CreateAnnouncementsRequest(models));
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAnnouncement(string id)
        {
            var viewModel = await mediator.Send(new GetAnnouncementByIdRequest(id));
            var existingPromotions = (await mediator.Send(new GetPromotionByStatusRequest(PromotionStatus.AllValue)))
                .Where(promotion => (!promotion.HasAnnouncement && promotion.Status.Equals(PromotionStatus.EnabledValue)) || promotion.PromotionId.Equals(viewModel.PromotionId))
                .ToList();
            viewModel.Promotions = existingPromotions;
            return PartialView("_AnnouncementModalItem", viewModel);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateAnnouncement([FromBody] CreateAnnouncementItemViewModel model)
        {
            var result = await mediator.Send(new UpdateAnnouncementRequest(model));
            return Json(result);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteAnnouncement(string id)
        {
            var result = await mediator.Send(new DeleteAnnouncementRequest(id));
            return Json(result);
        }
    }
}