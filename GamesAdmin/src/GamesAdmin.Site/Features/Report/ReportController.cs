using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.Requests;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Linq;

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
        public async Task<IActionResult> Index(GameId game)
        {
            var viewModel = await GetReportViewModel();
            viewModel.Game = game;

            return View("Index", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetReport(ReportViewModel viewModel, [FromServices] ReportView reportView)
        {
            var filter = new BetReportFilter
            {
                Game = viewModel.Game,
                RoundId = viewModel.RoundId ?? 0,
                PartnerId = viewModel.PartnerId,
                Nickname = viewModel.Nickname,
                Username = viewModel.Username,
                ConvertToUSD = viewModel.ConvertToUSD,
                ExcludeBot = viewModel.ExcludeBot,
                ExcludeUUS = viewModel.ExcludeUUS
            };
            return PartialView(reportView.GetView(viewModel.Game), await mediator.Send(new ReportRequest(filter)));
        }

        [HttpGet]
        public Task<IActionResult> BigSmall() => GetStakeBalancedView();

        [HttpGet]
        public async Task<IActionResult> GetBigSmallReport(
            long roundId,
            string nickname,
            bool excludeBot,
            string partnerId,
            string username,
            string currency)
        {
            var request = new GetBigSmallReportRequest(roundId, nickname, excludeBot, partnerId, username, currency);

            return PartialView("_BigSmallRecords", await mediator.Send(request));
        }

        [HttpGet]
        public Task<IActionResult> TurboBigSmall() => GetStakeBalancedView();

        [HttpGet]
        public async Task<IActionResult> GetBigSmallTurboReport(
            long roundId,
            string nickname,
            bool excludeBot,
            string partnerId,
            string username,
            string currency)
        {
            return PartialView("_BigSmallTurboRecords", await mediator.Send(new GetBigSmallTurboReportRequest(roundId, nickname, excludeBot, partnerId, username, currency)));
        }

        [HttpGet]
        public Task<IActionResult> OddEven() => GetStakeBalancedView();

        [HttpGet]
        public async Task<IActionResult> GetOddEvenReport(
            long roundId,
            string nickname,
            bool excludeBot,
            string partnerId,
            string username,
            string currency)
        {
            return PartialView("_OddEvenRecords", await mediator.Send(new GetOddEvenReportRequest(roundId, nickname, excludeBot, partnerId, username, currency)));
        }

        [HttpGet]
        public Task<IActionResult> TurboOddEven() => GetStakeBalancedView();

        [HttpGet]
        public async Task<IActionResult> GetOddEvenTurboReport(
            long roundId,
            string nickname,
            bool excludeBot,
            string partnerId,
            string username,
            string currency)
        {
            return PartialView("_OddEvenTurboRecords", await mediator.Send(new GetOddEvenTurboReportRequest(roundId, nickname, excludeBot, partnerId, username, currency)));
        }

        [HttpGet]
        public IActionResult SeDie()
        {
            return RedirectToAction("Index", new { game = GameId.SeDie });
        }

        [HttpGet]
        public IActionResult MiniSeDie()
        {
            return RedirectToAction("Index", new { game = GameId.MiniSeDie });
        }

        [HttpGet]
        public IActionResult FishPrawnCrab()
        {
            return RedirectToAction("Index", new { game = GameId.FishPrawnCrab });
        }

        [HttpGet]
        public IActionResult DragonTiger()
        {
            return RedirectToAction("Index", new { game = GameId.DragonTiger });
        }

        [HttpGet]
        public IActionResult Baccarat()
        {
            return RedirectToAction("Index", new { game = GameId.Baccarat });
        }

        [HttpGet]
        public IActionResult VipBaccarat()
        {
            return RedirectToAction("Index", new { game = GameId.VipBaccarat });
        }

        [HttpGet]
        [ActionName("rnglottery")]
        public IActionResult Lottery()
        {
            return RedirectToAction("Index", new { game = GameId.Lottery });
        }

        [HttpGet]
        public IActionResult SeDieWar()
        {
            return RedirectToAction("Index", new { game = GameId.VipSeDieWar });
        }

        [HttpGet]
        public IActionResult ThreeCards()
        {
            return RedirectToAction("Index", new { game = GameId.ThreeCards });
        }

        [HttpGet]
        [ActionName("kenomatch")]
        public IActionResult KenoMatch1()
        {
            return RedirectToAction("Index", new { game = GameId.KenoMatch1 });
        }

        [HttpGet]
        [ActionName("kenomatchmd5")]
        public IActionResult KenoMatch2()
        {
            return RedirectToAction("Index", new { game = GameId.KenoMatch2 });
        }

        [HttpGet]
        [ActionName("kenopower")]
        public IActionResult KenoPower1()
        {
            return RedirectToAction("Index", new { game = GameId.KenoPower1 });
        }

        [HttpGet]
        [ActionName("kenopowermd5")]
        public IActionResult KenoPower2()
        {
            return RedirectToAction("Index", new { game = GameId.KenoPower2 });
        }

        [HttpGet]
        public async Task<IActionResult> GetKenoPowerBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_KenoPowerBetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        [HttpGet]
        public async Task<IActionResult> GetKenoMatchBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_KenoMatchBetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        [HttpGet]
        public IActionResult Roulette()
        {
            return RedirectToAction("Index", new { game = GameId.Roulette });
        }

        [HttpGet]
        public async Task<IActionResult> GetRouletteBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_RouletteBetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        public IActionResult VipSeDie()
        {
            return RedirectToAction("Index", new { game = GameId.VipSeDie });
        }

        [HttpGet]
        public async Task<IActionResult> Blackjack()
        {
            var selections = await mediator.Send(new GetSelectionsRequest());

            return View(new BlackjackReportViewModel { ExcludeUUS = true, Selections = selections });

            return View(new BlackjackReportViewModel { ExcludeUUS = true, Selections = selections });
        }

        [HttpGet]
        public async Task<IActionResult> GetBlackjackReport(BlackjackBetReportFilter model)
        {
            return PartialView("_BlackjackRecords", await mediator.Send(new GetBlackjackReportRequest(model)));
        }

        [HttpGet]
        public Task<IActionResult> SpecialSeDie() => GetStakeBalancedView();

        [HttpGet]
        public IActionResult VipFishPrawnCrab()
        {
            return RedirectToAction("Index", new { game = GameId.VipFishPrawnCrab });
        }

        [HttpGet]
        public IActionResult SicBo()
        {
            return RedirectToAction("Index", new { game = GameId.SicBo });
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
        public async Task<IActionResult> GetVipSeDieWarBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_VipSeDieWarBetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecialSeDieReport(
            long roundId,
            string nickname,
            bool excludeBot,
            string partnerId,
            string username,
            string currency)
        {
            return PartialView("_SpecialSeDieRecords", await mediator.Send(new GetSpecialSeDieReportRequest(roundId, nickname, excludeBot, partnerId, username, currency)));
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecialSeDieBetHistory(long roundId, string userId, decimal? rate)
        {
            return PartialView("_SpecialSeDieBetHistory", await mediator.Send(new GetSpecialSeDieBetHistoryRequest(roundId, userId, rate)));
        }

        [HttpGet]
        public async Task<IActionResult> GetBigSmallBetHistory(long roundId, string nickname, decimal? rate)
        {
            return PartialView("_BigSmallBetHistory", await mediator.Send(new GetBigSmallBetHistoryRequest(roundId, nickname, rate)));
        }

        [HttpGet]
        public async Task<IActionResult> GetBigSmallTurboBetHistory(long roundId, string nickname, decimal? rate)
        {
            return PartialView("_BigSmallBetHistory", await mediator.Send(new GetBigSmallTurboBetHistoryRequest(roundId, nickname, rate)));
        }

        [HttpGet]
        public async Task<IActionResult> GetOddEvenBetHistory(long roundId, string nickname, decimal? rate)
        {
            return PartialView("_OddEvenBetHistory", await mediator.Send(new GetOddEvenBetHistoryRequest(roundId, nickname, rate)));
        }

        [HttpGet]
        public async Task<IActionResult> GetOddEvenTurboBetHistory(long roundId, string nickname, decimal? rate)
        {
            return PartialView("_OddEvenBetHistory", await mediator.Send(new GetOddEvenTurboBetHistoryRequest(roundId, nickname, rate)));
        }

        [HttpGet]
        public async Task<IActionResult> GetSicBoBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_SicBoBetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        [HttpGet]
        public async Task<IActionResult> GetLotteryBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_LotteryBetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        [HttpGet]
        public async Task<IActionResult> GetHiLoBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_HiLoBetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
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
        public IActionResult KenoProViet() => RedirectToAction("Index");

        [HttpGet]
        public IActionResult KenoMax() => RedirectToAction("Index", new { game = GameId.KenoMax });

        [HttpGet]
        public IActionResult KenoMax2() => RedirectToAction("Index", new { game = GameId.KenoMax2 });

        [HttpGet]
        public IActionResult KenoMini() => RedirectToAction("Index", new { game = GameId.KenoMini });

        [HttpGet]
        public IActionResult KenoMini2() => RedirectToAction("Index", new { game = GameId.KenoMini2 });

        [HttpGet]
        public IActionResult KenoEast() => RedirectToAction("Index", new { game = GameId.KenoEast });

        [HttpGet]
        public IActionResult KenoWest() => RedirectToAction("Index", new { game = GameId.KenoWest });

        [HttpGet]
        public IActionResult KenoSouth() => RedirectToAction("Index", new { game = GameId.KenoSouth });

        [HttpGet]
        public IActionResult KenoNorth() => RedirectToAction("Index", new { game = GameId.KenoNorth });

        [HttpGet]
        public IActionResult KenoProMax() => RedirectToAction("Index", new { game = GameId.KenoProMax });

        [HttpGet]
        public async Task<IActionResult> KenoBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return View(await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
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
        public Task<IActionResult> BitcoinMaster() => GetStakeBalancedView();

        [HttpGet]
        public async Task<IActionResult> GetBitcoinMasterReport(
            long roundId,
            string nickname,
            bool excludeBot,
            string partnerId,
            string username,
            string currency)
        {
            var request = new GetBitcoinMasterReportRequest(roundId, nickname, excludeBot, partnerId, username, currency);

            return PartialView("_BitcoinMasterRecords", await mediator.Send(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetBitcoinMasterBetHistory(long roundId, string nickname, decimal? rate)
            => PartialView("_BitcoinMasterBetHistory", await mediator.Send(new GetBitcoinMasterBetHistoryRequest(roundId, nickname, rate)));

        [HttpGet]
        [Route("Report/Hi-Lo")]
        public async Task<IActionResult> HiLo()
        {
            var selections = await mediator.Send(new GetSelectionsRequest());

            return View(new HiLoReportViewModel { ExcludeUUS = true, Selections = selections });
        }

        [HttpGet]
        public async Task<IActionResult> GetHiLoReport(HiLoBetReportFilter model)
        {
            return PartialView("_HiLoRecords", await mediator.Send(new GetHiLoReportRequest(model)));
       [HttpGet]
        [Route("Report/CasinoHoldem")]
        public async Task<IActionResult> CasinoHoldemPoker()
            => View("HoldemPoker/Index", await mediator.Send(new HoldemPokerReportRequest(GameId.CasinoHoldemPoker)));

        [HttpGet]
        [Route("Report/TexasHoldem")]
        public async Task<IActionResult> TexasHoldemPoker()
            => View("HoldemPoker/Index", await mediator.Send(new HoldemPokerReportRequest(GameId.TexasHoldemPoker)));

        [HttpGet]
        public async Task<IActionResult> GetHoldemPokerReport(HoldemPokerBetReportFilter filter)
            => PartialView("HoldemPoker/_Report", await mediator.Send(new GetHoldemPokerReportRequest(filter)));

        [HttpGet]
        public async Task<IActionResult> HoldemPokerBetHistory(HoldemPokerBetHistoryFilter filter)
            => PartialView("HoldemPoker/_BetHistory", await mediator.Send(new GetHoldemPokerBetHistoryRequest(filter)));
        }

        [HttpGet]
        public async Task<IActionResult> CasinoHoldemPokerBetHistory(long roundId, string nickname)
        {
            var viewmodel = await mediator.Send(new GetCasinoHoldemPokerBetHistoryRequest(roundId, nickname));

            return PartialView("_CasinoHoldemPokerBetHistory", viewmodel);
        }

        private async Task<ReportViewModel> GetReportViewModel()
        {
            var selections = await mediator.Send(new GetSelectionsRequest());
            var games = Game.GetNormalGames().Select(x => new SelectListItem(x.DisplayName, x.GameId.ToString()));
            return new ReportViewModel { ExcludeUUS = true, Selections = selections, Games = games };
        }
    }
}
