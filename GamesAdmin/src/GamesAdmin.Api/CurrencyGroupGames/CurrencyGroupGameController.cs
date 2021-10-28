using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.CurrencyGroupGames.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.CurrencyGroupGames
{
    [Route("api/currencyGroupGame")]
    public class CurrencyGroupGameController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public CurrencyGroupGameController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("update")]
        public Task<bool> Update(CurrencyGroupGame gameConfig)
           => mediator.Send(new UpdateCurrencyGroupGameRequest(gameConfig));

        [HttpGet("get")]
        public Task<CurrencyGroupGame> Get(int gameId, string currencyGroupId)
            => mediator.Send(new GetCurrenctGroupGameRequest(currencyGroupId, gameId));

        [HttpGet("getAll")]
        public Task<IEnumerable<CurrencyGroupGame>> GetAll()
            => mediator.Send(new GetAllRequest());

        [HttpGet("getByGroup")]
        public Task<IEnumerable<CurrencyGroupGame>> GetByGroup(string currencyGroupId)
            => mediator.Send(new GetByGroupRequest(currencyGroupId));
    }
}