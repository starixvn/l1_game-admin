using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
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

        [Get("/report/roulette/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<RouletteBetReport> GetRouletteReport(long roundId, string nickname, bool excludeUUS);

        [Get("/report/bet_history/game/roulette/round/{roundId}/user/{userId}")]
        Task<RouletteBetHistory> GetRouletteBetHistory(long roundId, string userId);

        [Get("/report/lucky_wheel/users/{nickname}")]
        Task<IEnumerable<SpinResult>> GetLuckyWheelBetHistory(string nickname);
        
        [Get("/report/vipsedie/round/{roundId}/users/{nickname}?excludeUUS={excludeUUS}")]
        Task<VipSeDieBetReport> GetVipSeDieReport(long roundId, string nickname, bool excludeUUS);
    }
}