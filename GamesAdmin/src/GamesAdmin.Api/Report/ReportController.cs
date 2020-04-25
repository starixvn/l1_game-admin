using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Report.Request;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Report
{
    [Route("api/report")]
    public class ReportController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public ReportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("big_small/round/{roundId}/users/{nickname?}")]
        public async Task<GameReport> BigSmall(long roundId, string nickname, bool excludeBot)
           => (await mediator.Send(new BigSmallReportRequest(roundId, nickname, excludeBot)));

        [HttpGet]
        [Route("bet_history/game/{gameTypeId}/round/{roundId}/user/{nickname}")]
        public async Task<IEnumerable<BaseBetHistory>> BetHistory(string nickname, long roundId, byte gameTypeId)
            => (await mediator.Send(new GameReportRequest(nickname, roundId, gameTypeId)));

        [HttpGet]
        [Route("big_small_turbo/round/{roundId}/users/{nickname?}")]
        public async Task<GameReport> BigSmallTurbo(long roundId, string nickname, bool excludeBot)
            => (await mediator.Send(new BigSmallTurboReportRequest(roundId, nickname, excludeBot)));

        [HttpGet]
        [Route("odd_even/round/{roundId}/users/{nickname?}")]
        public async Task<GameReport> OddEven(long roundId, string nickname, bool excludeBot)
            => (await mediator.Send(new OddEvenReportRequest(roundId, nickname, excludeBot)));

        [HttpGet]
        [Route("odd_even_turbo/round/{roundId}/users/{nickname?}")]
        public async Task<GameReport> OddEvenTurbo(long roundId, string nickname, bool excludeBot)
            => (await mediator.Send(new OddEvenTurboReportRequest(roundId, nickname, excludeBot)));
    }
}