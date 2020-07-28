using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Report.Request;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
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
        public async Task<BigSmallBetReport> BigSmall(long roundId, string nickname, bool excludeBot)
           => (await mediator.Send(new BigSmallReportRequest(roundId, nickname, excludeBot)));

        [HttpGet]
        [Route("bet_history/game/{gameTypeId}/round/{roundId}/user/{nickname}")]
        public async Task<IEnumerable<BaseBetHistory>> BetHistory(string nickname, long roundId, byte gameTypeId)
            => (await mediator.Send(new GameReportRequest(nickname, roundId, gameTypeId)));

        [HttpGet]
        [Route("big_small_turbo/round/{roundId}/users/{nickname?}")]
        public async Task<BigSmallBetReport> BigSmallTurbo(long roundId, string nickname, bool excludeBot)
            => (await mediator.Send(new BigSmallTurboReportRequest(roundId, nickname, excludeBot)));

        [HttpGet]
        [Route("odd_even/round/{roundId}/users/{nickname?}")]
        public async Task<OddEvenBetReport> OddEven(long roundId, string nickname, bool excludeBot)
            => (await mediator.Send(new OddEvenReportRequest(roundId, nickname, excludeBot)));

        [HttpGet]
        [Route("odd_even_turbo/round/{roundId}/users/{nickname?}")]
        public async Task<OddEvenBetReport> OddEvenTurbo(long roundId, string nickname, bool excludeBot)
            => (await mediator.Send(new OddEvenTurboReportRequest(roundId, nickname, excludeBot)));

        [HttpGet]
        [Route("sedie/round/{roundId}/users/{nickname?}")]
        public async Task<SeDieBetReport> SeDie(long roundId, string nickname, bool excludeBot)
           => (await mediator.Send(new SeDieReportRequest(roundId, nickname, excludeBot)));

        [HttpGet]
        [Route("mini_sedie/round/{roundId}/users/{nickname?}")]
        public async Task<SeDieBetReport> MiniSeDie(long roundId, string nickname, bool excludeBot)
            => (await mediator.Send(new MiniSeDieReportRequest(roundId, nickname, excludeBot)));

        [HttpGet]
        [Route("{game}/round/{roundId}/users/{nickname?}")]
        public Task<IEnumerable<KenoBetRecord>> KenoBetReport(string game, long roundId, string nickname, bool excludeUUS)
            => mediator.Send(new KenoReportRequest(game, roundId, nickname, excludeUUS));

        [HttpGet]
        [Route("{game}/round/{roundId}/user/{nickname}")]
        public Task<IEnumerable<KenoBetHistory>> KenoBetHistory(string game, long roundId, string nickname)
            => mediator.Send(new KenoBetHistoryRequest(game, roundId, nickname));

        [HttpGet]
        [Route("{game}/round/{roundId}/result")]
        public Task<KenoRoundResult> KenoKenoRoundResult(string game, long roundId)
            => mediator.Send(new KenoRoundResultRequest(game, roundId));

        [HttpGet]
        [Route("fish_prawn_crab/round/{roundId}/users/{nickname?}")]
        public async Task<FishPrawnCrabBetReport> FishPrawnCrab(long roundId, string nickname, bool excludeBot)
        => (await mediator.Send(new FishPrawnCrabReportRequest(roundId, nickname, excludeBot)));

        [HttpGet]
        [Route("dragon_tiger/round/{roundId}/users/{nickname?}")]
        public async Task<DragonTigerBetReport> DragonTiger(long roundId, string nickname, bool excludeBot)
        => (await mediator.Send(new DragonTigerReportRequest(roundId, nickname, excludeBot)));
    }
}