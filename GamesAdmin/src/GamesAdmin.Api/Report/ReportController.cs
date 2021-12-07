using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Report.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
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

        //[HttpGet]
        //[Route("sedie")]
        //public async Task<SeDieBetReport> SeDie([FromQuery] BetReportFilter filter)
        //   => (await mediator.Send(new SeDieReportRequest(filter)));

        //[HttpGet]
        //[Route("mini_sedie")]
        //public async Task<SeDieBetReport> MiniSeDie([FromQuery] BetReportFilter filter)
        //    => (await mediator.Send(new MiniSeDieReportRequest(filter)));

        [HttpGet]
        [Route("{game}/round/{roundId}/users/{nickname?}")]
        public Task<IEnumerable<KenoBetRecord>> KenoBetReport(string game, long roundId, string nickname, bool excludeUUS)
            => mediator.Send(new KenoReportRequest(game, roundId, nickname, excludeUUS));

        [HttpGet]
        [Route("{game}/round/{roundId}/user/{userId}")]
        public Task<IEnumerable<KenoBetHistory>> KenoBetHistory(string game, long roundId, string nickname)
            => mediator.Send(new KenoBetHistoryRequest(game, roundId, nickname));

        [HttpGet]
        [Route("{game}/round/{roundId}/result")]
        public Task<KenoRoundResult> KenoKenoRoundResult(string game, long roundId)
            => mediator.Send(new KenoRoundResultRequest(game, roundId));

        //[HttpGet]
        //[Route("fish_prawn_crab")]
        //public async Task<FishPrawnCrabBetReport> FishPrawnCrab([FromQuery]BetReportFilter filter)
        //=> (await mediator.Send(new FishPrawnCrabReportRequest(filter)));

        [HttpGet]
        [Route("dragon_tiger")]
        public async Task<DragonTigerBetReport> DragonTiger(long roundId, string nickname, bool excludeUUS)
        => (await mediator.Send(new DragonTigerReportRequest(roundId, nickname, excludeUUS)));

        [HttpGet]
        [Route("baccarat/round/{roundId}/users/{nickname?}")]
        public async Task<BaccaratBetReport> Baccarat(long roundId, string nickname, bool excludeUUS)
       => (await mediator.Send(new BaccaratReportRequest(roundId, nickname, excludeUUS)));

        [HttpGet]
        [Route("vip_baccarat/round/{roundId}/users/{nickname?}")]
        public async Task<VipBaccaratBetReport> VipBaccarat(long roundId, string nickname, bool excludeUUS)
      => (await mediator.Send(new VipBaccaratReportRequest(roundId, nickname, excludeUUS)));

        [HttpGet]
        [Route("roulette/round/{roundId}/users/{nickname?}")]
        public async Task<RouletteBetReport> Roulette(long roundId, string nickname, bool excludeUUS)
        => (await mediator.Send(new RouletteReportRequest(roundId, nickname, excludeUUS)));

        [HttpGet]
        [Route("bet_history/game/roulette/round/{roundId}/user/{userId}")]
        public async Task<RouletteBetHistory> BetHistory(string userId, long roundId)
         => (await mediator.Send(new RouletteBetHistoryRequest(roundId, userId)));

        [HttpGet]
        [Route("lucky_wheel/users/{nickname}")]
        public async Task<IEnumerable<SpinResult>> LuckyWheel(string nickname)
        => (await mediator.Send(new LuckyWheelReportRequest(nickname)));

        [HttpGet]
        [Route("vipsedie/round/{roundId}/users/{nickname?}")]
        public async Task<VipSeDieBetReport> VipSeDie(long roundId, string nickname, bool excludeUUS)
        => (await mediator.Send(new VipSeDieReportRequest(roundId, nickname, excludeUUS)));

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
        [Route("vip_fish_prawn_crab/round/{roundId}/users/{nickname?}")]
        public async Task<VipFishPrawnCrabBetReport> VipFishPrawnCrab(long roundId, string nickname, bool excludeUUS)
        => (await mediator.Send(new VipFishPrawnCrabReportRequest(roundId, nickname, excludeUUS)));

        [HttpGet]
        [Route("vipsediewar/round/{roundId}/users/{nickname?}")]
        public async Task<VipSeDieWarBetReport> VipSeDieWar(long roundId, string nickname, bool excludeUUS)
        => (await mediator.Send(new VipSeDieWarReportRequest(roundId, nickname, excludeUUS)));

        [HttpGet]
        [Route("bet_history/game/vipsediewar/round/{roundId}/user/{userId}")]
        public async Task<VipSeDieWarBetHistory> VipSeDieWarBetHistory(string userId, long roundId)
         => (await mediator.Send(new VipSeDieWarBetHistoryRequest(roundId, userId)));

        [HttpGet]
        [Route("sic_bo/round/{roundId}/users/{nickname?}")]
        public async Task<SicBoBetReport> SicBo(long roundId, string nickname, bool excludeUUS)
        => (await mediator.Send(new SicBoReportRequest(roundId, nickname, excludeUUS)));

        [HttpGet]
        [Route("bet_history/game/sicbo/round/{roundId}/user/{userId}")]
        public async Task<SicBoBetHistory> SicBoBetHistory(long roundId, string userId)
        => (await mediator.Send(new SicBoBetHistoryRequest(roundId, userId)));

        [HttpGet]
        [Route("jackpot/game/{gameId}")]
        public async Task<IEnumerable<JackpotRecord>> JackpotReport(byte gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot)
       => (await mediator.Send(new JackpotReportRequest(gameId, fromDate, toDate, excludeUUS, excludeBot)));

        [HttpGet]
        [Route("lottery/round/{roundId}/users/{nickname?}")]
        public async Task<LotteryBetReport> Lottery(long roundId, string nickname, bool excludeUUS)
            => (await mediator.Send(new LotteryReportRequest(roundId, nickname, excludeUUS)));

        [HttpGet]
        [Route("bet_history/game/lottery/round/{roundId}/user/{userId}")]
        public async Task<LotteryBetHistory> LotteryBetHistory(long roundId, string userId)
        => (await mediator.Send(new LotteryBetHistoryRequest(roundId, userId)));

        [HttpGet]
        [Route("threecards/round/{roundId}/users/{nickname?}")]
        public async Task<ThreeCardsBetReport> ThreeCards(long roundId, string nickname, bool excludeUUS)
            => (await mediator.Send(new ThreeCardsReportRequest(roundId, nickname, excludeUUS)));

        [HttpGet]
        [Route("bitcoin_master/round/{roundId}/users/{nickname?}")]
        public async Task<BitcoinMasterBetReport> BitcoinMaster(long roundId, string nickname, bool excludeBot, string partnerId, string username)
           => (await mediator.Send(new BitcoinMasterReportRequest(roundId, nickname, excludeBot, partnerId, username)));

        [HttpGet]
        [Route("bitcoin_master/round/{roundId}/user/{nickname}")]
        public async Task<BitcoinMasterBetHistory> BitcoinMasterBetHistory(long roundId, string nickname)
            => await mediator.Send(new BitcoinMasterBetHistoryRequest(roundId, nickname));

        [HttpGet]
        [Route("kenomatch/{game}/round/{roundId}/users/{nickname?}")]
        public Task<KenoMatchBetReport> KenoMatch(GameId game, long roundId, string nickname, bool excludeUUS)
            => mediator.Send(new KenoMatchReportRequest(game, roundId, nickname, excludeUUS));

        [HttpGet]
        [Route("bet_history/kenomatch/{game}/round/{roundId}/user/{userId}")]
        public async Task<KenoMatchBetHistory> KenoMatchBetHistory(GameId game, string userId, long roundId)
        => (await mediator.Send(new KenoMatchBetHistoryRequest(game, roundId, userId)));

        [HttpGet]
        [Route("kenopower/{game}/round/{roundId}/users/{nickname?}")]
        public Task<KenoPowerBetReport> KenoPower(GameId game, long roundId, string nickname, bool excludeUUS)
          => mediator.Send(new KenoPowerReportRequest(game, roundId, nickname, excludeUUS));

        [HttpGet]
        [Route("bet_history/kenopower/{game}/round/{roundId}/user/{userId}")]
        public async Task<KenoPowerBetHistory> KenoPowerBetHistory(GameId game, string userId, long roundId)
        => (await mediator.Send(new KenoPowerBetHistoryRequest(game, roundId, userId)));

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
