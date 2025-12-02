using System;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.PlayerChainResults.ViewModels;
using GamesAdmin.Api.PlayerChainResults.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.PlayerChainResults
{
    [Route("api/player-chain-results")]
    public class PlayerChainResultController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public PlayerChainResultController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayerChainResults([FromQuery] PlayerChainResultsQuery query)
        {
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
            return Ok(response);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportPlayerChainResults([FromQuery] PlayerChainResultsQuery query)
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
                
                if (excelData == null || excelData.Length == 0)
                {
                    return BadRequest("No data available for export with the specified criteria.");
                }

                var fileName = $"PlayerChainResults_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                
                return File(excelData, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while exporting data: {ex.Message}");
            }
        }
    }
}
