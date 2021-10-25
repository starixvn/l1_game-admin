using Audit.Mvc;
using GamesAdmin.Core.Models.Partners;
using GamesAdmin.Site.Features.Partners.Requests;
using GamesAdmin.Site.Features.Partners.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sentry;
using System;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Partners
{
    [Authorize(Roles = "admin")]
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
        public async Task<IActionResult> Index() => View(await mediator.Send(new IndexRequest()));

        [HttpGet]
        public IActionResult Add() => PartialView();

        [HttpPost]
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
                    addViewModel.ApiUrl,
                    addViewModel.TestCurrencyUrl)));


            return Json(addedResult);
        }

        [HttpGet]
        [Route("partner/{partnerId}")]
        public async Task<IActionResult> EditView(string partnerId)
            => PartialView("Edit", await mediator.Send(new GetByIdRequest(partnerId)));

        [HttpPost, Audit]
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
                   viewModel.ApiUrl,
                   viewModel.TestCurrencyUrl));

                return Json(result);
            }
            catch(Exception ex)
            {
                sentryClient.CaptureException(ex);
                return Json(false);
            }
           
        }
    }
}
