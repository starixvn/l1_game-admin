using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.Report.Requests;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.Report
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IMediator mediator;

        public ReportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public IActionResult BigSmall()
        {
            return View(new ReportViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetBigSmallReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_BigSmallRecords", await mediator.Send(new GetBigSmallReportRequest(roundId, nickname, excludeBot)));
        }

        [HttpGet]
        public IActionResult TurboBigSmall()
        {
            return View(new ReportViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetBigSmallTurboReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_BigSmallTurboRecords", await mediator.Send(new GetBigSmallTurboReportRequest(roundId, nickname, excludeBot)));
        }

        [HttpGet]
        public IActionResult OddEven()
        {
            return View(new ReportViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetOddEvenReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_OddEvenRecords", await mediator.Send(new GetOddEvenReportRequest(roundId, nickname, excludeBot)));
        }

        [HttpGet]
        public IActionResult TurboOddEven()
        {
            return View(new ReportViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetOddEvenTurboReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_OddEvenTurboRecords", await mediator.Send(new GetOddEvenTurboReportRequest(roundId, nickname, excludeBot)));
        }

        [HttpGet]
        public IActionResult SeDie()
        {
            return View(new ReportViewModel { ExcludeUUS = true });
        }

        [HttpGet]
        public IActionResult MiniSeDie()
        {
            return View(new ReportViewModel { ExcludeUUS = true });
        }

        [HttpGet]
        public IActionResult FishPrawnCrab()
        {
            return View(new ReportViewModel { ExcludeUUS = true });
        }

        [HttpGet]
        public IActionResult DragonTiger()
        {
            return View(new ReportViewModel { ExcludeUUS = true });
        }

        [HttpGet]
        public IActionResult Baccarat()
        {
            return View(new ReportViewModel { ExcludeUUS = true });
        }

        [HttpGet]
        public IActionResult VipBaccarat()
        {
            return View(new ReportViewModel { ExcludeUUS = true });
        }


        [HttpGet]
        [ActionName("rnglottery")]
        public IActionResult Lottery()
        {
            return View("Lottery", new ReportViewModel { ExcludeUUS = true });
        }

        [HttpGet]
        public IActionResult SeDieWar()
        {
            return View("VipSeDieWar", new ReportViewModel { ExcludeUUS = true });
        }

        [HttpGet]
        public IActionResult ThreeCards()
        {
            return View(new ReportViewModel { ExcludeUUS = true });
        }

        [HttpGet]
        public IActionResult MyDinh()
        {
            ViewData["Title"] = "My Dinh";
            return View("KenoSoccer", new KenoSoccerReportViewModel { Game = GameId.MyDinh, ExcludeUUS = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetKenoSoccerReport(GameId game, long roundId, string nickname, bool excludeUUS)
        {
            return PartialView("_KenoSoccerRecords", await mediator.Send(new GetKenoSoccerReportRequest(game, roundId, nickname, excludeUUS)));
        }

        [HttpGet]
        public async Task<IActionResult> GetThreeCardsReport(long roundId, string nickname, bool excludeUUS)
        {
            return PartialView("_ThreeCardsRecords", await mediator.Send(new GetThreeCardsReportRequest(roundId, nickname, excludeUUS)));
        }

        [HttpGet]
        public async Task<IActionResult> GetBaccaratReport(long roundId, string nickname, bool excludeUUS)
        {
            return PartialView("_BaccaratRecords", await mediator.Send(new GetBaccaratReportRequest(roundId, nickname, excludeUUS)));
        }

        [HttpGet]
        public async Task<IActionResult> GetVipBaccaratReport(long roundId, string nickname, bool excludeUUS)
        {
            return PartialView("_VipBaccaratRecords", await mediator.Send(new GetVipBaccaratReportRequest(roundId, nickname, excludeUUS)));
        }

        [HttpGet]
        public IActionResult Roulette()
        {
            return View(new ReportViewModel { ExcludeUUS = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetRouletteReport(long roundId, string nickname, bool excludeUUS)
        {
            return PartialView("_RouletteRecords", await mediator.Send(new GetRouletteReportRequest(roundId, nickname, excludeUUS)));
        }

        [HttpGet]
        public async Task<IActionResult> GetRouletteBetHistory(long roundId, string userId)
        {
            return PartialView("_RouletteBetHistory", await mediator.Send(new GetRouletteBetHistoryRequest(roundId, userId)));
        }

        [HttpGet]
        public async Task<IActionResult> GetSeDieReport(long roundId, string nickname, bool excludeUUS)
        {
            return PartialView("_SeDieRecords", await mediator.Send(new GetSeDieReportRequest(roundId, nickname, excludeUUS)));
        }

        [HttpGet]
        public IActionResult VipSeDie()
        {
            return View(new ReportViewModel { ExcludeUUS = true });
        }

        [HttpGet]
        public IActionResult Blackjack()
        {
            return View(new BlackjackReportViewModel { ExcludeUUS = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetBlackjackReport(long handId, string nickname, bool excludeUUS)
        {
            return PartialView("_BlackjackRecords", await mediator.Send(new GetBlackjackReportRequest(handId, nickname, excludeUUS)));
        }

        [HttpGet]
        public async Task<IActionResult> GetVipSeDieReport(long roundId, string nickname, bool excludeUUS)
        {
            return PartialView("_VipSeDieRecords", await mediator.Send(new GetVipSeDieReportRequest(roundId, nickname, excludeUUS)));
        }

        [HttpGet]
        public IActionResult SpecialSeDie()
        {
            return View(new ReportViewModel());
        }

        [HttpGet]
        public IActionResult VipFishPrawnCrab()
        {
            return View(new ReportViewModel { ExcludeUUS = true });
        }

        [HttpGet]
        public IActionResult SicBo()
        {
            return View(new ReportViewModel { ExcludeUUS = true });
        }

        [HttpGet]
        public IActionResult FourGuardians()
        {
            return View(new ReportViewModel { ExcludeUUS = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetSlotGameReport(long roundId, string nickname, bool excludeUUS, string game)
        {
            return PartialView("_SlotGameRecords", await mediator.Send(new GetSlotGameReportRequest(roundId, nickname, excludeUUS, game)));
        }

        [HttpGet]
        public async Task<IActionResult> GetSlotGameBetHistory(long roundId, string nickname, bool excludeUUS, string game)
        {
            return PartialView("_SlotGameBetHistory",
                (await mediator.Send(new GetSlotGameReportRequest(roundId, nickname, excludeUUS, game))).Records.FirstOrDefault());
        }

        [HttpGet]
        public async Task<IActionResult> GetSicBoReport(long roundId, string nickname, bool excludeUUS)
        {
            return PartialView("_SicBoRecords", await mediator.Send(new GetSicBoReportRequest(roundId, nickname, excludeUUS)));
        }

        [HttpGet]
        public async Task<IActionResult> GetLotteryReport(long roundId, string nickname, bool excludeUUS)
        {
            return PartialView("_LotteryRecords", await mediator.Send(new GetLotteryReportRequest(roundId, nickname, excludeUUS)));
        }

        [HttpGet]
        public async Task<IActionResult> GetVipSeDieWarReport(long roundId, string nickname, bool excludeUUS)
        {
            return PartialView("_VipSeDieWarRecords", await mediator.Send(new GetVipSeDieWarReportRequest(roundId, nickname, excludeUUS)));
        }

        [HttpGet]
        public async Task<IActionResult> GetVipSeDieWarBetHistory(long roundId, string userId)
        {
            return PartialView("_VipSeDieWarBetHistory", await mediator.Send(new GetVipSeDieWarBetHistoryRequest(roundId, userId)));
        }

        [HttpGet]
        public async Task<IActionResult> GetVipFishPrawnCrabReport(long roundId, string nickname, bool excludeUUS)
        {
            return PartialView("_VipFishPrawnCrabRecords", await mediator.Send(new GetVipFishPrawnCrabReportRequest(roundId, nickname, excludeUUS)));
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecialSeDieReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_SpecialSeDieRecords", await mediator.Send(new GetSpecialSeDieReportRequest(roundId, nickname, excludeBot)));
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecialSeDieBetHistory(long roundId, string userId)
        {
            return PartialView("_SpecialSeDieBetHistory", await mediator.Send(new GetSpecialSeDieBetHistoryRequest(roundId, userId)));
        }

        [HttpGet]
        public async Task<IActionResult> GetDragonTigerReport(long roundId, string nickname, bool excludeUUS)
        {
            return PartialView("_DragonTigerRecords", await mediator.Send(new GetDragonTigerReportRequest(roundId, nickname, excludeUUS)));
        }

        [HttpGet]
        public async Task<IActionResult> GetFishPrawnCrabReport(long roundId, string nickname, bool excludeUUS)
        {
            return PartialView("_FishPrawnCrabRecords", await mediator.Send(new GetFishPrawnCrabReportRequest(roundId, nickname, excludeUUS)));
        }

        [HttpGet]
        public async Task<IActionResult> GetMiniSeDieReport(long roundId, string nickname, bool excludeUUS)
        {
            return PartialView("_MiniSeDieRecords", await mediator.Send(new GetMiniSeDieReportRequest(roundId, nickname, excludeUUS)));
        }

        [HttpGet]
        public async Task<IActionResult> GetBigSmallBetHistory(long roundId, string nickname)
        {
            return PartialView("_BigSmallBetHistory", await mediator.Send(new GetBigSmallBetHistoryRequest(roundId, nickname)));
        }

        [HttpGet]
        public async Task<IActionResult> GetBigSmallTurboBetHistory(long roundId, string nickname)
        {
            return PartialView("_BigSmallBetHistory", await mediator.Send(new GetBigSmallTurboBetHistoryRequest(roundId, nickname)));
        }

        [HttpGet]
        public async Task<IActionResult> GetOddEvenBetHistory(long roundId, string nickname)
        {
            return PartialView("_OddEvenBetHistory", await mediator.Send(new GetOddEvenBetHistoryRequest(roundId, nickname)));
        }

        [HttpGet]
        public async Task<IActionResult> GetOddEvenTurboBetHistory(long roundId, string nickname)
        {
            return PartialView("_OddEvenBetHistory", await mediator.Send(new GetOddEvenTurboBetHistoryRequest(roundId, nickname)));
        }

        [HttpGet]
        public async Task<IActionResult> GetSicBoBetHistory(long roundId, string userId)
        {
            return PartialView("_SicBoBetHistory", await mediator.Send(new GetSicBoBetHistoryRequest(roundId, userId)));
        }

        [HttpGet]
        public async Task<IActionResult> GetLotteryBetHistory(long roundId, string userId)
        {
            return PartialView("_LotteryBetHistory", await mediator.Send(new GetLotteryBetHistoryRequest(roundId, userId)));
        }

        [HttpGet]
        public IActionResult HeroesSlot()
        {
            return View(new ReportViewModel { ExcludeUUS = true });
        }

        [HttpGet]
        public IActionResult KenoProViet() => ToKenoReport();

        [HttpGet]
        public IActionResult KenoMax() => ToKenoReport();

        [HttpGet]
        public IActionResult KenoMax2() => ToKenoReport();

        [HttpGet]
        public IActionResult KenoMini() => ToKenoReport();

        [HttpGet]
        public IActionResult KenoMini2() => ToKenoReport();

        [HttpGet]
        public IActionResult KenoEast() => ToKenoReport();

        [HttpGet]
        public IActionResult KenoWest() => ToKenoReport();

        [HttpGet]
        public IActionResult KenoSouth() => ToKenoReport();

        [HttpGet]
        public IActionResult KenoNorth() => ToKenoReport();

        [HttpGet]
        public IActionResult KenoProMax() => ToKenoReport();

        private IActionResult ToKenoReport() => View("KenoReport", new KenoViewModel());

        [HttpGet]
        public async Task<IActionResult> GetKenoReport(string game, long roundId, string nickname = null, bool excludeUUS = true)
        {
            return PartialView("_KenoBetRecords", await mediator.Send(new GetKenoReportRequest(
                Enumeration.FromValue<Game>(game),
                roundId,
                nickname,
                excludeUUS)));
        }

        [HttpGet]
        public async Task<IActionResult> KenoBetHistory(string game, long roundId, string nickname)
        {
            return View(await mediator.Send(new GetKenoBetHistoryRequest(Enumeration.FromValue<Game>(game), roundId, nickname)));
        }

        [HttpGet]
        public IActionResult LuckyWheel()
        {
            return View(new LuckyWheelViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> LuckyWheelHistory(string nickname)
        {
            return PartialView("_LuckyWheelRecords", await mediator.Send(new GetLuckyWheelHistoryRequest(nickname)));
        }

        [HttpGet]
        public async Task<IActionResult> GetJackpotReport(JackpotReportViewModel model)
        {
            return PartialView("_JackpotRecords",
                await mediator.Send(
                    new GetJackpotReportRequest(model.GameId,
                        model.FromDate.Date.ToUniversalTime(),
                        model.ToDate.Date.AddDays(1).AddTicks(-1).ToUniversalTime(),
                        model.ExcludeUUS, model.ExcludeBot)));
        }
    }
}