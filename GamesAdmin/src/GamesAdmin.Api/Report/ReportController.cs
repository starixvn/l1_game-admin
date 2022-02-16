using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Report.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Core.Models.Spins;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        [Route("")]
        public async Task<BetReportBase> Index([FromQuery] BetReportFilter filter)
           => (await mediator.Send(new BetReportRequest(filter)));

        [HttpGet]
        [Route("bethistory")]
        public async Task<BetHistory> BetHistory([FromQuery] BetHistoryFilter filter)
        => (await mediator.Send(new BetHistoryRequest(filter)));

        [HttpGet]
        [Route("big_small/round/{roundId}/users/{nickname?}")]
        public async Task<BigSmallBetReport> BigSmall(long roundId, string nickname, bool excludeBot, string partnerId, string username)
           => (await mediator.Send(new BigSmallReportRequest(roundId, nickname, excludeBot, partnerId, username)));

        [HttpGet]
        [Route("bet_history/game/{gameTypeId}/round/{roundId}/user/{nickname}")]
        public async Task<IEnumerable<BaseBetHistory>> BetHistory(string nickname, long roundId, byte gameTypeId)
            => (await mediator.Send(new GameReportRequest(nickname, roundId, gameTypeId)));

        [HttpGet]
        [Route("big_small_turbo/round/{roundId}/users/{nickname?}")]
        public async Task<BigSmallBetReport> BigSmallTurbo(long roundId, string nickname, bool excludeBot, string partnerId, string username)
            => (await mediator.Send(new BigSmallTurboReportRequest(roundId, nickname, excludeBot, partnerId, username)));

        [HttpGet]
        [Route("odd_even/round/{roundId}/users/{nickname?}")]
        public async Task<OddEvenBetReport> OddEven(long roundId, string nickname, bool excludeBot, string partnerId, string username)
            => (await mediator.Send(new OddEvenReportRequest(roundId, nickname, excludeBot, partnerId, username)));

        [HttpGet]
        [Route("odd_even_turbo/round/{roundId}/users/{nickname?}")]
        public async Task<OddEvenBetReport> OddEvenTurbo(long roundId, string nickname, bool excludeBot, string partnerId, string username)
            => (await mediator.Send(new OddEvenTurboReportRequest(roundId, nickname, excludeBot, partnerId, username)));

        [HttpGet]
        [Route("{game}/round/{roundId}/user/{userId}")]
        public Task<IEnumerable<KenoBetHistoryRecord>> KenoBetHistory(string game, long roundId, string nickname)
            => mediator.Send(new KenoBetHistoryRequest(game, roundId, nickname));

        [HttpGet]
        [Route("lucky_wheel/users/{nickname}")]
        public async Task<IEnumerable<SpinResult>> LuckyWheel(string nickname)
        => (await mediator.Send(new LuckyWheelReportRequest(nickname)));

        [HttpGet]
        [Route("specialsedie/round/{roundId}/users/{nickname?}")]
        public async Task<SpecialSeDieBetReport> SpecialSedie(long roundId, string nickname, bool excludeBot, string partnerId, string username)
        => (await mediator.Send(new SpecialSeDieReportRequest(roundId, nickname, excludeBot, partnerId, username)));

        [HttpGet]
        [Route("bet_history/game/specialsedie/round/{roundId}/user/{userId}")]
        public async Task<SpecialSeDieBetHistory> SpecialSeDieBetHistory(long roundId, string userId)
         => (await mediator.Send(new SpecialSeDieBetHistoryRequest(roundId, userId)));

        [HttpGet]
        [Route("blackjack")]
        public async Task<IEnumerable<BlackjackBetRecord>> Blackjack([FromQuery] BlackjackBetReportFilter filter)
        => (await mediator.Send(new BlackjackReportRequest(filter)));

        [HttpGet]
        [Route("slot_game/round/{roundId}/nick/{nickname?}")]
        public async Task<IEnumerable<SlotGameBetRecord>> SlotGame(long roundId,
            string nickname,
            bool excludeUUS,
            string game,
            string partnerId,
            DateTime fromDate,
            DateTime toDate,
            string username,
            bool convertToUSD)
        {
            var filter = new SlotGameBetReportFilter
            {
                RoundId = roundId,
                Nickname = nickname,
                ExcludeUUS = excludeUUS,
                Game = Enumeration.FromValue<Game>(game),
                PartnerId = partnerId,
                FromDate = fromDate,
                ToDate = toDate,
                Username = username,
                ConvertToUSD = convertToUSD,
            };

            return await mediator.Send(new SlotGameReportRequest(filter));
        }

        [HttpGet]
        [Route("jackpot/game/{gameId}")]
        public async Task<IEnumerable<JackpotRecord>> JackpotReport(byte gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot)
       => (await mediator.Send(new JackpotReportRequest(gameId, fromDate, toDate, excludeUUS, excludeBot)));

        [HttpGet]
        [Route("bitcoin_master/round/{roundId}/users/{nickname?}")]
        public async Task<BitcoinMasterBetReport> BitcoinMaster(long roundId, string nickname, bool excludeBot, string partnerId, string username)
           => (await mediator.Send(new BitcoinMasterReportRequest(roundId, nickname, excludeBot, partnerId, username)));

        [HttpGet]
        [Route("bitcoin_master/round/{roundId}/user/{nickname}")]
        public async Task<BitcoinMasterBetHistory> BitcoinMasterBetHistory(long roundId, string nickname)
            => await mediator.Send(new BitcoinMasterBetHistoryRequest(roundId, nickname));

        [HttpGet]
        [Route("casinoholdempoker")]
        public async Task<IEnumerable<CasinoHoldemPokerBetRecord>> CasinoHoldemPoker([FromQuery] CasinoHoldemPokerBetReportFilter filter)
       => (await mediator.Send(new CasinoHoldemPokerReportRequest(filter)));

        [HttpGet]
        [Route("bet_history/casinoholdempoker")]
        public async Task<CasinoHoldemPokerBetHistory> CasinoHoldemPokerBetHistory([FromQuery] CasinoHoldemPokerBetHistoryFilter filter)
         => (await mediator.Send(new CasinoHoldemPokerBetHistoryRequest(filter)));
    }
}
