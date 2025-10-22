using System;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
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
        public async Task<IActionResult> GetPlayerChainResults([FromQuery] string gameId = null, [FromQuery] string playerId = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 25)
        {
            var request = new GetPlayerChainResultsRequest
            {
                GameId = gameId,
                PlayerId = playerId,
                Page = page,
                PageSize = pageSize
            };

            var response = await mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportPlayerChainResults([FromQuery] string gameId = null, [FromQuery] string playerId = null)
        {
            try
            {
                var request = new ExportPlayerChainResultsRequest
                {
                    GameId = gameId,
                    PlayerId = playerId
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
