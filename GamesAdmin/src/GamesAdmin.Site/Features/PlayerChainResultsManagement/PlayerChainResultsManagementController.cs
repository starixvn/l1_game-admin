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

        public async Task<IActionResult> Index(PlayerChainResultsQuery query)
        {
            // Default to today if no dates provided
            if (!query.FromDate.HasValue && !query.ToDate.HasValue)
            {
                var today = DateTime.Today;
                query.FromDate = today;
                query.ToDate = today;
            }

            if (!ModelState.IsValid)
            {
                return View(new IndexViewModel
                {
                    Results = new List<PlayerChainResultViewModel>(),
                    GameId = query.GameId,
                    PlayerId = query.PlayerId,
                    FromDate = query.FromDate,
                    ToDate = query.ToDate,
                    ExcludeUUS = query.ExcludeUUS,
                    CurrentPage = query.Page,
                    PageSize = query.PageSize
                });
            }

            var request = new GetPlayerChainResultsRequest
            {
                GameId = query.GameId,
                PlayerId = query.PlayerId,
                FromDate = query.FromDate,
                ToDate = query.ToDate,
                ExcludeUUS = query.ExcludeUUS,
                Page = query.Page,
                PageSize = query.PageSize
            };

            var response = await mediator.Send(request);

            var viewModel = new IndexViewModel
            {
                Results = response.Results,
                GameId = query.GameId,
                PlayerId = query.PlayerId,
                FromDate = query.FromDate,
                ToDate = query.ToDate,
                ExcludeUUS = query.ExcludeUUS,
                CurrentPage = response.CurrentPage,
                PageSize = response.PageSize,
                TotalPages = response.TotalPages,
                TotalRecords = response.TotalRecords
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Export(PlayerChainResultsQuery query)
        {
            try
            {
                var request = new ExportPlayerChainResultsRequest
                {
                    GameId = query.GameId,
                    PlayerId = query.PlayerId,
                    FromDate = query.FromDate,
                    ToDate = query.ToDate,
                    ExcludeUUS = query.ExcludeUUS
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
