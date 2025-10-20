using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.PlayerChainResultsManagement.Requests;
using GamesAdmin.Site.Features.PlayerChainResultsManagement.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.PlayerChainResultsManagement
{
    [Authorize]
    public class PlayerChainResultsManagementController : Controller
    {
        private readonly IMediator mediator;

        public PlayerChainResultsManagementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index(string gameId = null, string playerId = null, int page = 1, int pageSize = 25)
        {
            if (!ModelState.IsValid)
            {
                return View(new IndexViewModel
                {
                    Results = new List<PlayerChainResultViewModel>(),
                    GameId = gameId,
                    PlayerId = playerId,
                    CurrentPage = page,
                    PageSize = pageSize
                });
            }

            var request = new GetPlayerChainResultsRequest
            {
                GameId = gameId,
                PlayerId = playerId,
                Page = page,
                PageSize = pageSize
            };

            var response = await mediator.Send(request);

            var viewModel = new IndexViewModel
            {
                Results = response.Results,
                GameId = gameId,
                PlayerId = playerId,
                CurrentPage = response.CurrentPage,
                PageSize = response.PageSize,
                TotalPages = response.TotalPages,
                TotalRecords = response.TotalRecords
            };

            return View(viewModel);
        }
    }
}
