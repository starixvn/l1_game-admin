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

        [HttpGet]
        public IActionResult CreateTournament()
        {
            var viewModel = new PromotionCampaignViewModel();
            viewModel.WinningPrizes = new List<WinningPrizeViewModel>
        {
            new WinningPrizeViewModel { Ranking = "1", Value = 0 },
            new WinningPrizeViewModel { Ranking = "2", Value = 0 },
            new WinningPrizeViewModel { Ranking = "3", Value = 0 }
        };
            return PartialView("_AddTournament", viewModel);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateTournament([Bind] PromotionCampaignViewModel model)
        {
            var result = await mediator.Send(new CreateTournamentRequest(model));

            return Json(result);
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

        public async Task<IActionResult> GetAnnouncementByStatus(string status, string type)
        => PartialView("_AnnouncementTable", await mediator.Send(new GetAnnouncementByStatusRequest(status, type)));

        [HttpGet]
        public async Task<IActionResult> CreateAnnouncement()
        {
            var viewModel = new CreateAnnouncementItemViewModel();
            viewModel.Promotions = (await mediator.Send(new GetPromotionByStatusRequest(PromotionStatus.Enabled.Value)))
                .Where(promotion => !promotion.HasAnnouncement);
            viewModel.PromotionCampaigns = (await mediator.Send(new GetTournamentPromotionsByStatusRequest(PromotionCampaignStatus.All.Value)))
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
            var existingPromotionCampaigns = (await mediator.Send(new GetTournamentPromotionsByStatusRequest(PromotionCampaignStatus.AllValue)))
                .Where(promotion => (!promotion.HasAnnouncement) || promotion.Id.Equals(viewModel.PromotionId))
                .ToList();
            viewModel.Promotions = existingPromotions;
            viewModel.PromotionCampaigns = existingPromotionCampaigns;
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

        public IActionResult Leaderboard(int promotionId)
        {
            var viewModel = new LeaderboardViewModel(promotionId);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetLeaderboard(int promotionId, string playerName, bool isTestCurrency)
        {
            var request = new GetLeaderboardRequest(promotionId, playerName, isTestCurrency);
            var viewModel = await mediator.Send(request);

            return PartialView("_LeaderboardTable", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetTournamentWinningPlayers(int promotionId)
        {
            var viewModel = await mediator.Send(new GetTournamentWinningPlayersRequest(promotionId));

            return PartialView("_TournamentWinningPlayers", viewModel);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AwardTournament(int promotionId)
        {
            var request = new AwardTournamentRequest(promotionId);
            var result = await mediator.Send(request);

            return Json(result);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AwardTestCurrency(int promotionId, string playerId, string operatorId, double awardAmount)
        {
            var request = new AwardTestCurrencyRequest(promotionId, playerId, operatorId, awardAmount);
            var result = await mediator.Send(request);

            return Json(result);
        }

        [HttpGet]
        public IActionResult ViewAwardTestCurrency(string operatorId, string operatorName, string playerId, string playerName, string currency, double point, double awardAmount)
        {
            var viewModel = new AwardTestCurrencyViewModel
            {
                OperatorId = operatorId,
                OperatorName= operatorName,
                PlayerId = playerId,
                PlayerName = playerName,
                Currency = currency,
                Point = point,
                AwardAmount = awardAmount
            };

            return PartialView("_AwardTestCurrency", viewModel);
        }

        public async Task<IActionResult> UpdateOperatorSettings(string id, string operatorId, string operatorName, string status)
            => PartialView("_UpdateOperatorSetting", await mediator.Send(new GetUpdateOperatorSettingRequest(id, operatorId, operatorName, status)));

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateOperatorSettings(EditCurrencyGroupSettingViewModel model)
        {
            var result = await mediator.Send(new UpdateOperatorSettingRequest(model));

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrencyGroupSetting(string id, string operatorId, string groupId, string groupName)
           => PartialView("_EditCurrencyGroupSetting", await mediator.Send(new GetCurrencySettingRequest(id, operatorId, groupId)));

        [HttpGet]
        public async Task<IActionResult> UpdateTournament(string id)
        {
            var viewModel = await mediator.Send(new GetTournamentByIdRequest(id));
            return PartialView("_UpdateTournament", viewModel);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateTournament(PromotionCampaignViewModel model)
        {
            var result = await mediator.Send(new UpdateTournamentRequest(model));

            return Json(result);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteTournament(string id)
        {
            var result = await mediator.Send(new DeleteTournamentRequest(id));

            return Json(result);
        }
    }
}