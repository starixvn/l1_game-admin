using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Core.Models.Spins;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.Report
{
    public interface IReportApi : IBaseAuthorizationApi
    {
        [Get("/report/big_small/round/{roundId}/users/{nickname}?excludeBot={excludeBot}")]
        Task<BigSmallBetReport> GetBigSmallReport(long roundId, string nickname, bool excludeBot);

        [Get("/report/big_small_turbo/round/{roundId}/users/{nickname}?excludeBot={excludeBot}")]
        Task<BigSmallBetReport> GetBigSmallTurboReport(long roundId, string nickname, bool excludeBot);

        [Get("/report/odd_even/round/{roundId}/users/{nickname}?excludeBot={excludeBot}")]
        Task<OddEvenBetReport> GetOddEvenReport(long roundId, string nickname, bool excludeBot);

        [Get("/report/odd_even_turbo/round/{roundId}/users/{nickname}?excludeBot={excludeBot}")]
        Task<OddEvenBetReport> GetOddEvenTurboReport(long roundId, string nickname, bool excludeBot);

        [Get("/report/sedie/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<SeDieBetReport> GetSeDieReport(long roundId, string nickname, bool excludeUUS);

        [Get("/report/mini_sedie/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<SeDieBetReport> GetMiniSeDieReport(long roundId, string nickname, bool excludeUUS);

        [Get("/report/bet_history/game/{gameTypeId}/round/{roundId}/user/{nickname}")]
        Task<IEnumerable<T>> GetBetHistory<T>(byte gameTypeId, long roundId, string nickname) where T : BaseBetHistory;

        [Get("/report/{game}/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<IEnumerable<KenoBetRecord>> GetKenoReport(string game, long roundId, string nickname, bool excludeUUS);

        [Get("/report/{game}/round/{roundId}/user/{nickname}")]
        Task<IEnumerable<KenoBetHistory>> GetKenoBetHistory(string game, long roundId, string nickname);

        [Get("/report/{game}/round/{roundId}/result")]
        Task<KenoRoundResult> GetKenoRoundResult(string game, long roundId);

        [Get("/report/fish_prawn_crab/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<FishPrawnCrabBetReport> GetFishPrawnCrabReport(long roundId, string nickname, bool excludeUUS);

        [Get("/report/dragon_tiger/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<DragonTigerBetReport> GetDragonTigerReport(long roundId, string nickname, bool excludeUUS);

        [Get("/report/baccarat/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<BaccaratBetReport> GetBaccaratReport(long roundId, string nickname, bool excludeUUS);

        [Get("/report/vip_baccarat/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<VipBaccaratBetReport> GetVipBaccaratReport(long roundId, string nickname, bool excludeUUS);

        [Get("/report/roulette/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<RouletteBetReport> GetRouletteReport(long roundId, string nickname, bool excludeUUS);

        [Get("/report/bet_history/game/roulette/round/{roundId}/user/{userId}")]
        Task<RouletteBetHistory> GetRouletteBetHistory(long roundId, string userId);

        [Get("/report/lucky_wheel/users/{nickname}")]
        Task<IEnumerable<SpinResult>> GetLuckyWheelBetHistory(string nickname);

        [Get("/report/vipsedie/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<VipSeDieBetReport> GetVipSeDieReport(long roundId, string nickname, bool excludeUUS);

        [Get("/report/specialsedie/round/{roundId}/users/{nickname}?excludeBot={excludeBot}")]
        Task<SpecialSeDieBetReport> GetSpecialSeDieReport(long roundId, string nickname, bool excludeBot);

        [Get("/report/bet_history/game/specialsedie/round/{roundId}/user/{userId}")]
        Task<SpecialSeDieBetHistory> GetSpecialSeDieBetHistory(long roundId, string userId);

        [Get("/report/blackjack/hand/{handId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<IEnumerable<BlackjackBetRecord>> GetBlackjackReport(long handId, string nickname, bool excludeUUS);

        [Get("/report/vip_fish_prawn_crab/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<VipFishPrawnCrabBetReport> GetVipFishPrawnCrabReport(long roundId, string nickname, bool excludeUUS);

        [Get("/report/sic_bo/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<SicBoBetReport> GetSicBoBetReport(long roundId, string nickname, bool excludeUUS);

        [Get("/report/vipsediewar/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<VipSeDieWarBetReport> GetVipSeDieWarReport(long roundId, string nickname, bool excludeUUS);

        [Get("/report/bet_history/game/sicbo/round/{roundId}/user/{userId}")]
        Task<SicBoBetHistory> GetSicBoBetHistory(long roundId, string userId);

        [Get("/report/bet_history/game/vipsediewar/round/{roundId}/user/{userId}")]
        Task<VipSeDieWarBetHistory> GetVipSeDieWarBetHistory(long roundId, string userId);

        [Get("/report/slot_game/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<IEnumerable<SlotGameBetRecord>> GetSlotGameReport(long roundId, string nickname, bool excludeUUS, string game);

        [Get("/report/jackpot/game/{gameId}?excludeUUS={excludeUUS}&excludeBot={excludeBot}")]
        Task<IEnumerable<JackpotRecord>> GetJackpotReport(byte gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot);

        [Get("/report/lottery/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<LotteryBetReport> GetLotteryBetReport(long roundId, string nickname, bool excludeUUS);

        [Get("/report/bet_history/game/lottery/round/{roundId}/user/{userId}")]
        Task<LotteryBetHistory> GetLotteryBetHistory(long roundId, string userId);

        [Get("/report/threecards/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<ThreeCardsBetReport> GetThreeCardsBetReport(long roundId, string nickname, bool excludeUUS);

        [Get("/report/bitcoin_master/round/{roundId}/users/{nickname}?excludeBot={excludeBot}")]
        Task<BitcoinMasterBetReport> GetBitcoinMasterReport(long roundId, string nickname, bool excludeBot);

        [Get("/report/bitcoin_master/round/{roundId}/user/{nickname}")]
        Task<BitcoinMasterBetHistory> GetBitcoinMasterBetReport(long roundId, string nickname);

       [Get("/report/kenomatch/{game}/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<KenoMatchBetReport> GetKenoMatchBetReport(string game, long roundId, string nickname, bool excludeUUS);

        [Get("/report/bet_history/kenomatch/{game}/round/{roundId}/user/{userId}")]
        Task<KenoMatchBetHistory> GetKenoMatchBetHistory(string game, long roundId, string userId);

   	[Get("/report/kenopower/{game}/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<KenoPowerBetReport> GetKenoPowerBetReport(string game, long roundId, string nickname, bool excludeUUS);

        [Get("/report/bet_history/kenopower/{game}/round/{roundId}/user/{userId}")]
        Task<KenoPowerBetHistory> GetKenoPowerBetHistory(string game, long roundId, string userId);
    }
}