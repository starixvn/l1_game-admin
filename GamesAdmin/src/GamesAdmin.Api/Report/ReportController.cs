using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Report.Requests;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Core.Models.Spins;
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
        public async Task<SeDieBetReport> SeDie(long roundId, string nickname, bool excludeUUS)
           => (await mediator.Send(new SeDieReportRequest(roundId, nickname, excludeUUS)));

        [HttpGet]
        [Route("mini_sedie/round/{roundId}/users/{nickname?}")]
        public async Task<SeDieBetReport> MiniSeDie(long roundId, string nickname, bool excludeUUS)
            => (await mediator.Send(new MiniSeDieReportRequest(roundId, nickname, excludeUUS)));

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
        public async Task<FishPrawnCrabBetReport> FishPrawnCrab(long roundId, string nickname, bool excludeUUS)
        => (await mediator.Send(new FishPrawnCrabReportRequest(roundId, nickname, excludeUUS)));

        [HttpGet]
        [Route("dragon_tiger/round/{roundId}/users/{nickname?}")]
        public async Task<DragonTigerBetReport> DragonTiger(long roundId, string nickname, bool excludeUUS)
        => (await mediator.Send(new DragonTigerReportRequest(roundId, nickname, excludeUUS)));

        [HttpGet]
        [Route("baccarat/round/{roundId}/users/{nickname?}")]
        public async Task<BaccaratBetReport> Baccarat(long roundId, string nickname, bool excludeUUS)
       => (await mediator.Send(new BaccaratReportRequest(roundId, nickname, excludeUUS)));

        [HttpGet]
        [Route("roulette/round/{roundId}/users/{nickname?}")]
        public async Task<RouletteBetReport> Roulette(long roundId, string nickname, bool excludeUUS)
        => (await mediator.Send(new RouletteReportRequest(roundId, nickname, excludeUUS)));

        [HttpGet]
        [Route("bet_history/game/roulette/round/{roundId}/user/{userId}")]
        public async Task<RouletteBetHistory> BetHistory(string userId, long roundId)
         => (await mediator.Send(new RouletteBetHistoryRequest(roundId, userId)));

        [HttpGet]
        [Route("luckywheel/users/{nickname}")]
        public async Task<IEnumerable<SpinResult>> LuckyWheel(string nickname)
        => (await mediator.Send(new LuckyWheelReportRequest(nickname)));

        [HttpGet]
        [Route("vipsedie/round/{roundId}/users/{nickname?}")]
        public async Task<VipSeDieBetReport> VipSeDie(long roundId, string nickname, bool excludeUUS)
        => (await mediator.Send(new VipSeDieReportRequest(roundId, nickname, excludeUUS)));

        [HttpGet]
        [Route("specialsedie/round/{roundId}/users/{nickname?}")]
        public async Task<SpecialSeDieBetReport> SpecialSedie(long roundId, string nickname, bool excludeBot)
        => (await mediator.Send(new SpecialSeDieReportRequest(roundId, nickname, excludeBot)));

        [HttpGet]
        [Route("bet_history/game/specialsedie/round/{roundId}/user/{userId}")]
        public async Task<SpecialSeDieBetHistory> SpecialSeDieBetHistory(long roundId, string userId)
         => (await mediator.Send(new SpecialSeDieBetHistoryRequest(roundId, userId)));
         
         [HttpGet]
        [Route("blackjack/hand/{handId}/users/{nickname?}")]
        public async Task<IEnumerable<BlackjackBetRecord>> Blackjack(long handId, string nickname, bool excludeUUS)
        => (await mediator.Send(new BlackjackReportRequest(handId, nickname, excludeUUS)));

    }
}