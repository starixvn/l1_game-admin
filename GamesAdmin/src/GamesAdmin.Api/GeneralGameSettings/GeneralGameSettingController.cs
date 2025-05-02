using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.GeneralGameSettings.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.GeneralGameSettings
{
    [Route("api/general_game_settings")]
    public class GeneralGameSetingController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public GeneralGameSetingController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("getAll")]
        public Task<IEnumerable<GeneralGameSetting>> GetAll()
        => mediator.Send(new GetAllRequest());

        [HttpGet("games")]
        public Task<IEnumerable<GeneralGameSetting>> GetByCurrencyGroups(
            [FromQuery] int gameType,
            [FromQuery] string currencyGroups)
        {
            return mediator.Send(new GetGameByCurrencyGroupsRequest(gameType, currencyGroups));
        }

    }
}