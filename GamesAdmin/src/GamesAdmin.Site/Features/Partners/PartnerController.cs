using System;
using System.Threading.Tasks;
using Audit.Mvc;
using GamesAdmin.Core.Models.Partners;
using GamesAdmin.Site.Features.Partners.Requests;
using GamesAdmin.Site.Features.Partners.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sentry;

namespace GamesAdmin.Site.Features.Partners
{
    [Authorize]
    public class PartnerController : Controller
    {
        private readonly IMediator mediator;
        private readonly ISentryClient sentryClient;

        public PartnerController(IMediator mediator, ISentryClient sentryClient)
        {
            this.mediator = mediator;
            this.sentryClient = sentryClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = await mediator.Send(new IndexRequest());

            viewModel.EditPermission = HttpContext.User.IsInRole("admin");

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Add() => PartialView();

        [HttpPost, Audit]
        public async Task<IActionResult> Add([Bind] AddViewModel addViewModel)
        {
            var addedResult = await mediator.Send(new AddPartnerRequest(
                new Partner(
                    addViewModel.PartnerId,
                    addViewModel.Name,
                    addViewModel.SecretKey,
                    addViewModel.ClubVersionUrl,
                    addViewModel.MobiVersionUrl,
                    addViewModel.MiniVersionUrl,
                    addViewModel.MegaKenoUrl,
                    addViewModel.MobileDirectUrl,
                    addViewModel.TestCurrencyUrl,
                    addViewModel.LivestreamingPlatformUrl,
                    addViewModel.OriginalGamesUrl,
                    addViewModel.IsDisplayPlayerCurrency,
                    addViewModel.Alias)));

            return Json(addedResult);
        }

        [HttpGet]
        [Route("partner/{partnerId}")]
        public async Task<IActionResult> EditView(string partnerId)
            => PartialView("Edit", await mediator.Send(new GetByIdRequest(partnerId)));

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit([Bind] PartnerEditViewModel viewModel)
        {
            try
            {
                var result = await mediator.Send(new UpdateRequest(
                   viewModel.PartnerId,
                   viewModel.Name,
                   viewModel.SecretKey,
                   viewModel.ClubVersionUrl,
                   viewModel.MobiVersionUrl,
                   viewModel.MiniVersionUrl,
                   viewModel.MegaKenoUrl,
                   viewModel.MobileDirectUrl,
                   viewModel.TestCurrencyUrl,
                   viewModel.LivestreamingPlatformUrl,
                   viewModel.OriginalGamesUrl,
                   viewModel.IsDisplayPlayerCurrency,
                   viewModel.Alias));

                return Json(result);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return Json(false);
            }
        }

        [HttpGet]
        [Route("partner/site_exclusion")]
        public async Task<IActionResult> GetSiteExclusion(string partnerId, int gameId)
            => Json(await mediator.Send(new GetSiteExclusionRequest(partnerId, gameId)));

        [HttpGet]
        [Route("partner/edit_super_players")]
        public async Task<IActionResult> SuperPlayers()
            => PartialView("SuperPlayers", await mediator.Send(new GetSuperPlayerViewRequest()));

        [HttpGet]
        [Route("partner/super_players")]
        public async Task<IActionResult> GetSuperPlayers(string partnerId)
            => Json(await mediator.Send(new GetSuperPlayerRequest(partnerId)));

        [HttpPost, Audit]
        [Route("partner/site_exclusion")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateSiteExclusion(string partnerId, int gameId, string[] exclusionSites, int[] otherGameIdsInGroup)
            => Json(await mediator.Send(new UpdateSiteExclusionRequest(partnerId, gameId, exclusionSites, otherGameIdsInGroup)));

        [HttpPost, Audit]
        [Route("partner/edit_super_players")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateSuperPlayer(string partnerId, string[] superPlayers)
            => Json(await mediator.Send(new UpdateSuperPlayerRequest(partnerId, superPlayers)));

        [HttpPost, Audit]
        [Route("partner/um")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdatePartnerUM(bool isUM, DateTimeOffset startTime, DateTimeOffset endTime, string[] partners)
            => Json(await mediator.Send(new UpdatePartnerUMRequest(isUM, startTime, endTime, partners)));
    }
}