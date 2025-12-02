using System;
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

        public async Task<IActionResult> Index(string gameId = null, string playerId = null, DateTime? fromDate = null, DateTime? toDate = null, bool excludeUUS = false, int page = 1, int pageSize = 25)
        {
            // Default to today if no dates provided
            if (!fromDate.HasValue && !toDate.HasValue)
            {
                var today = DateTime.Today;
                fromDate = today;
                toDate = today;
            }

            if (!ModelState.IsValid)
            {
                return View(new IndexViewModel
                {
                    Results = new List<PlayerChainResultViewModel>(),
                    GameId = gameId,
                    PlayerId = playerId,
                    FromDate = fromDate,
                    ToDate = toDate,
                    ExcludeUUS = excludeUUS,
                    CurrentPage = page,
                    PageSize = pageSize
                });
            }

            var request = new GetPlayerChainResultsRequest
            {
                GameId = gameId,
                PlayerId = playerId,
                FromDate = fromDate,
                ToDate = toDate,
                ExcludeUUS = excludeUUS,
                Page = page,
                PageSize = pageSize
            };

            var response = await mediator.Send(request);

            var viewModel = new IndexViewModel
            {
                Results = response.Results,
                GameId = gameId,
                PlayerId = playerId,
                FromDate = fromDate,
                ToDate = toDate,
                ExcludeUUS = excludeUUS,
                CurrentPage = response.CurrentPage,
                PageSize = response.PageSize,
                TotalPages = response.TotalPages,
                TotalRecords = response.TotalRecords
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Export(string gameId = null, string playerId = null, DateTime? fromDate = null, DateTime? toDate = null, bool excludeUUS = false)
        {
            try
            {
                var request = new ExportPlayerChainResultsRequest
                {
                    GameId = gameId,
                    PlayerId = playerId,
                    FromDate = fromDate,
                    ToDate = toDate,
                    ExcludeUUS = excludeUUS
                };

                var excelData = await mediator.Send(request);

                var fileName = $"PlayerChainResults_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                
                return File(excelData, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while exporting data: " + ex.Message);
            }
        }
    }
}
