using System;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.Requests;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index()
        {
            var viewModel = await GetReportViewModel();
            return View("Index", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetReport(ReportViewModel viewModel, [FromServices] ReportView reportView)
        {
            var filter = new BetReportFilter
            {
                RoundId = viewModel.RoundId ?? 0,
                PartnerId = viewModel.PartnerId,
                Nickname = viewModel.Nickname,
                Username = viewModel.Username,
                ConvertToUSD = viewModel.ConvertToUSD,
                ExcludeBot = viewModel.ExcludeBot,
                ExcludeUUS = viewModel.ExcludeUUS
            };
            return PartialView(reportView.GetView(viewModel.Game), await mediator.Send(new ReportRequest(viewModel.Game, filter)));
        }

        [HttpGet]
        public Task<IActionResult> BigSmall() => GetStakeBalancedView();

        [HttpGet]
        public async Task<IActionResult> GetBigSmallReport(
            long roundId,
            string nickname,
            bool excludeBot,
            string partner,
            string username,
            string currency)
        {
            var request = new GetBigSmallReportRequest(roundId, nickname, excludeBot, partner, username, currency);

            return PartialView("_BigSmallRecords", await mediator.Send(request));
        }

        [HttpGet]
        public Task<IActionResult> TurboBigSmall() => GetStakeBalancedView();

        [HttpGet]
        public async Task<IActionResult> GetBigSmallTurboReport(
            long roundId,
            string nickname,
            bool excludeBot,
            string partner,
            string username,
            string currency)
        {
            return PartialView("_BigSmallTurboRecords", await mediator.Send(new GetBigSmallTurboReportRequest(roundId, nickname, excludeBot, partner, username, currency)));
        }

        [HttpGet]
        public Task<IActionResult> OddEven() => GetStakeBalancedView();

        [HttpGet]
        public async Task<IActionResult> GetOddEvenReport(
            long roundId,
            string nickname,
            bool excludeBot,
            string partner,
            string username,
            string currency)
        {
            return PartialView("_OddEvenRecords", await mediator.Send(new GetOddEvenReportRequest(roundId, nickname, excludeBot, partner, username, currency)));
        }

        [HttpGet]
        public Task<IActionResult> TurboOddEven() => GetStakeBalancedView();

        [HttpGet]
        public async Task<IActionResult> GetOddEvenTurboReport(
            long roundId,
            string nickname,
            bool excludeBot,
            string partner,
            string username,
            string currency)
        {
            return PartialView("_OddEvenTurboRecords", await mediator.Send(new GetOddEvenTurboReportRequest(roundId, nickname, excludeBot, partner, username, currency)));
        }

        [HttpGet]
        public IActionResult SeDie()
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> MiniSeDie()
        {
            return View(await GetReportViewModel());
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
        [ActionName("kenomatch")]
        public IActionResult KenoMatch1()
        {
            ViewData["Title"] = "Keno Match";
            return View("KenoMatch", new KenoMatchReportViewModel { Game = GameId.KenoMatch1, ExcludeUUS = true });
        }

        [HttpGet]
        [ActionName("kenomatchmd5")]
        public IActionResult KenoMatch2()
        {
            ViewData["Title"] = "Keno Match MD5";
            return View("KenoMatch", new KenoMatchReportViewModel { Game = GameId.KenoMatch2, ExcludeUUS = true });
        }

        [HttpGet]
        [ActionName("kenopower")]
        public IActionResult KenoPower1()
        {
            ViewData["Title"] = "Keno Power";
            return View("KenoPower", new KenoPowerReportViewModel { Game = GameId.KenoPower1, ExcludeUUS = true });
        }

        [HttpGet]
        [ActionName("kenopowermd5")]
        public IActionResult KenoPower2()
        {
            ViewData["Title"] = "Keno Power MD5";
            return View("KenoPower", new KenoPowerReportViewModel { Game = GameId.KenoPower2, ExcludeUUS = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetKenoPowerReport(GameId game, long roundId, string nickname, bool excludeUUS)
        {
            return PartialView("_KenoPowerRecords", await mediator.Send(new GetKenoPowerReportRequest(game, roundId, nickname, excludeUUS)));
        }

        [HttpGet]
        public async Task<IActionResult> GetKenoPowerBetHistory(GameId game, long roundId, string userId)
        {
            return PartialView("_KenoPowerBetHistory", await mediator.Send(new GetKenoPowerBetHistoryRequest(game, roundId, userId)));
        }

        [HttpGet]
        public async Task<IActionResult> GetKenoMatchBetHistory(GameId game, long roundId, string userId)
        {
            return PartialView("_KenoMatchBetHistory", await mediator.Send(new GetKenoMatchBetHistoryRequest(game, roundId, userId)));
        }

        [HttpGet]
        public async Task<IActionResult> GetKenoMatchReport(GameId game, long roundId, string nickname, bool excludeUUS)
        {
            return PartialView("_KenoMatchRecords", await mediator.Send(new GetKenoMatchReportRequest(game, roundId, nickname, excludeUUS)));
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
        public async Task<IActionResult> GetSeDieReport(BetReportFilter filter)
        {
            return PartialView("_SeDieRecords", await mediator.Send(new GetSeDieReportRequest(filter)));
        }

        [HttpGet]
        public IActionResult VipSeDie()
        {
            return View(new ReportViewModel { ExcludeUUS = true });
        }

        [HttpGet]
        public async Task<IActionResult> Blackjack()
        {
            var selections = await mediator.Send(new GetSelectionsRequest());

            return View(new BlackjackReportViewModel { ExcludeUUS = true, Selections = selections });
        }

        [HttpGet]
        public async Task<IActionResult> GetBlackjackReport(BlackjackBetReportFilter model)
        {
            return PartialView("_BlackjackRecords", await mediator.Send(new GetBlackjackReportRequest(model)));
        }

        [HttpGet]
        public async Task<IActionResult> GetVipSeDieReport(long roundId, string nickname, bool excludeUUS)
        {
            return PartialView("_VipSeDieRecords", await mediator.Send(new GetVipSeDieReportRequest(roundId, nickname, excludeUUS)));
        }

        [HttpGet]
        public Task<IActionResult> SpecialSeDie() => GetStakeBalancedView();

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
        public async Task<IActionResult> FourGuardians()
        {
            var selections = await mediator.Send(new GetSelectionsRequest());
            var viewModel = new SlotGameReportViewModel
            {
                ExcludeUUS = true,
                Selections = selections
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetSlotGameReport(
            string partnerId,
            DateTime fromDate,
            DateTime toDate,
            long roundId,
            string nickname,
            string username,
            bool convertToUSD,
            bool excludeUUS,
            string game)
        {
            var request = new GetSlotGameReportRequest(
                roundId,
                nickname,
                excludeUUS,
                game,
                partnerId,
                fromDate,
                toDate,
                username,
                convertToUSD);

            return PartialView("_SlotGameRecords", await mediator.Send(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetSlotGameBetHistory(
            long roundId,
            string nickname,
            bool excludeUUS,
            string game,
            bool convertToUSD)
        {
            var request = new GetSlotGameReportRequest(roundId, nickname, excludeUUS, game, convertToUSD);

            return PartialView("_SlotGameBetHistory", (await mediator.Send(request)).Records.FirstOrDefault());
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
        public async Task<IActionResult> GetSpecialSeDieReport(
            long roundId,
            string nickname,
            bool excludeBot,
            string partner,
            string username,
            string currency)
        {
            return PartialView("_SpecialSeDieRecords", await mediator.Send(new GetSpecialSeDieReportRequest(roundId, nickname, excludeBot, partner, username, currency)));
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
        public async Task<IActionResult> GetMiniSeDieReport(BetReportFilter filter)
        {
            return PartialView("_MiniSeDieRecords", await mediator.Send(new GetMiniSeDieReportRequest(filter)));
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
        public async Task<IActionResult> HeroesSlot()
        {
            var selections = await mediator.Send(new GetSelectionsRequest());
            var viewModel = new SlotGameReportViewModel
            {
                ExcludeUUS = true,
                Selections = selections
            };

            return View(viewModel);
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

        private async Task<IActionResult> GetStakeBalancedView()
        {
            var selections = await mediator.Send(new GetSelectionsRequest());

            return View(new StakeBalancedViewModel { Selections = selections });
        }

        [HttpGet]
        public IActionResult BitcoinMaster()
        {
            return View(new BitcoinMasterViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetBitcoinMasterReport(long roundId, string nickname, bool excludeBot)
        {
            return PartialView("_BitcoinMasterRecords", await mediator.Send(new GetBitcoinMasterReportRequest(roundId, nickname, excludeBot)));
        }

        [HttpGet]
        public async Task<IActionResult> GetBitcoinMasterBetHistory(long roundId, string nickname)
            => PartialView("_BitcoinMasterBetHistory", await mediator.Send(new GetBitcoinMasterBetHistoryRequest(roundId, nickname)));

        [HttpGet]
        [Route("Report/CasinoHoldem")]
        public async Task<IActionResult> CasinoHoldemPoker()
        {
            var selections = await mediator.Send(new GetSelectionsRequest());

            return View(new CasinoHoldemPokerReportViewModel { ExcludeUUS = true, Selections = selections });
        }

        [HttpGet]
        public async Task<IActionResult> GetCasinoHoldemPokerReport(CasinoHoldemPokerBetReportFilter filter)
        {
            return PartialView("_CasinoHoldemPokerRecords", await mediator.Send(new GetCasinoHoldemPokerReportRequest(filter)));
        }

        [HttpGet]
        public async Task<IActionResult> CasinoHoldemPokerBetHistory(CasinoHoldemPokerBetHistoryFilter filter)
        {
            var viewmodel = await mediator.Send(new GetCasinoHoldemPokerBetHistoryRequest(filter));

            return PartialView("_CasinoHoldemPokerBetHistory", viewmodel);
        }
        
        private async Task<ReportViewModel> GetReportViewModel()
        {
            var selections = await mediator.Send(new GetSelectionsRequest());
            var games = Enumeration.GetAll<Game>().Select(x => new SelectListItem(x.DisplayName, x.GameId.ToString()));
            return new ReportViewModel { ExcludeUUS = true, Selections = selections, Games = games };
        }
    }
}
