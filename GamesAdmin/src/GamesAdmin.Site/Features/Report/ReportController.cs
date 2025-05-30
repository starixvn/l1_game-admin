using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site._Shared.Services.GeneralGameSettings;
using GamesAdmin.Site.Features.Report.Requests;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Report
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IMediator mediator;
        private readonly ISystemSettings systemSettings;
        private readonly IGeneralGameSettingService generalGameSettingService;
        private readonly IReportService reportService;

        public ReportController(IMediator mediator, ISystemSettings systemSettings, IGeneralGameSettingService generalGameSettingService, IReportService reportService)
        {
            this.mediator = mediator;
            this.systemSettings = systemSettings;
            this.generalGameSettingService = generalGameSettingService;
            this.reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(GameId game)
        {
            var viewModel = await GetReportViewModel();
            viewModel.Game = game;
            viewModel.BaseCurrency = systemSettings.BaseCurrency;

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
            var view = reportView.GetView(viewModel.Game);
            
            return view == reportView.RemoteRecordView ?
                PartialView(view, await reportService.GetRemoteRoundHistoryUrl(filter)) :
                PartialView(view, await mediator.Send(new ReportRequest(filter)));            
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
        public IActionResult FishPrawnCrab()
        {
            return RedirectToAction("Index", new { game = GameId.FishPrawnCrab });
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
        [ActionName("kenosoccer")]
        public IActionResult KenoMatch1()
        {
            return RedirectToAction("Index", new { game = GameId.KenoMatch1 });
        }

        [HttpGet]
        [ActionName("kenosoccer2")]
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
        [ActionName("kenopower2")]
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
        public async Task<IActionResult> GetRouletteBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_RouletteBetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        [HttpGet]
        public async Task<IActionResult> Blackjack()
        {
            var selections = await mediator.Send(new GetSelectionsRequest());

            return View(new BlackjackGameReportViewModel { BaseCurrency = this.systemSettings.BaseCurrency, ExcludeUUS = true, Selections = selections });
        }

        [HttpGet]
        public async Task<IActionResult> Pontoon()
        {
            var selections = await mediator.Send(new GetSelectionsRequest());

            return View(new BlackjackGameReportViewModel { BaseCurrency = this.systemSettings.BaseCurrency, ExcludeUUS = true, Selections = selections });
        }

        [HttpGet]
        public async Task<IActionResult> ChineseBlackjack()
        {
            var selections = await mediator.Send(new GetSelectionsRequest());

            return View(new BlackjackGameReportViewModel { BaseCurrency = this.systemSettings.BaseCurrency, ExcludeUUS = true, Selections = selections });
        }

        [HttpGet]
        public async Task<IActionResult> GetChineseBlackjackReport(BlackjackBetReportFilter model)
        {
            return PartialView("_ChineseBlackjackRecords", await mediator.Send(new GetChineseBlackjackReportRequest(model)));
        }

        [HttpGet]
        public async Task<IActionResult> GetBlackjackReport(BlackjackBetReportFilter model)
        {
            return PartialView("_BlackjackRecords", await mediator.Send(new GetBlackjackReportRequest(model)));
        }

        [HttpGet]
        public async Task<IActionResult> GetPontoonReport(BlackjackBetReportFilter model)
        {
            return PartialView("_PontoonRecords", await mediator.Send(new GetPontoonReportRequest(model)));
        }

        [HttpGet]
        public async Task<IActionResult> SoloBlackjack()
        {
            var selections = await mediator.Send(new GetSelectionsRequest());

            return View(new BlackjackGameReportViewModel { BaseCurrency = this.systemSettings.BaseCurrency, ExcludeUUS = true, Selections = selections });
        }

        [HttpGet]
        public async Task<IActionResult> GetSoloBlackjackReport(BlackjackBetReportFilter model)
        {
            return PartialView("_SoloBlackjackRecords", await mediator.Send(new GetSoloBlackjackReportRequest(model)));
        }

        [HttpGet]
        public Task<IActionResult> SpecialSeDie() => GetStakeBalancedView();

        [HttpGet]
        public async Task<IActionResult> SlotGameReport(string game)
        {
            var selections = await mediator.Send(new GetSelectionsRequest());
            var games = await generalGameSettingService.GetAllGames();
            var gameSetting = games.FirstOrDefault(gameInfo => gameInfo.Name == game);

            var viewModel = new SlotGameReportViewModel
            {
                ExcludeUUS = true,
                Selections = selections,
                BaseCurrency = systemSettings.BaseCurrency,
                SystemType = systemSettings.System,
                GameConfigName = game,
                GameDisplayName = gameSetting?.DisplayName,
                IsEnabledJackpot = gameSetting.IsEnabledJackpot,
                GameId = gameSetting.GameId
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
                        model.ExcludeUUS, model.ExcludeBot, model.ConvertToUSD, model.BaseCurrency)));
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
        [Route("Report/CasinoHoldem")]
        public async Task<IActionResult> CasinoHoldemPoker()
           => View("HoldemPoker/Index", await mediator.Send(new HoldemPokerReportRequest(GameId.CasinoHoldemPoker)));

        [HttpGet]
        [Route("Report/Hi-Lo")]
        public async Task<IActionResult> HiLo()
        {
            var selections = await mediator.Send(new GetSelectionsRequest());

            return View(new HiLoReportViewModel
            {
                BaseCurrency = systemSettings.BaseCurrency,
                ExcludeUUS = true,
                Selections = selections
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetHiLoReport(HiLoBetReportFilter model)
            => PartialView("_HiLoRecords", await mediator.Send(new GetHiLoReportRequest(model)));

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

        [HttpGet]
        public async Task<IActionResult> CascadingSlotGameReport(string game)
        {
            var selections = await mediator.Send(new GetSelectionsRequest());
            var games = await generalGameSettingService.GetAllGames();
            var gameSetting = games.FirstOrDefault(gameInfo => gameInfo.Name == game);

            var viewModel = new SlotGameReportViewModel
            {
                ExcludeUUS = true,
                Selections = selections,
                BaseCurrency = systemSettings.BaseCurrency,
                SystemType = systemSettings.System,
                GameConfigName = game,
                GameDisplayName = gameSetting?.DisplayName,
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetCascadingSlotReport(
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
            var request = new GetCascadingSlotReportRequest(
                roundId,
                nickname,
                excludeUUS,
                game,
                partnerId,
                fromDate,
                toDate,
                username,
                convertToUSD);

            return PartialView("CascadingSlots/_CascadingSlotRecords", await mediator.Send(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetCascadingSlotBetHistory(
            long roundId,
            string nickname,
            bool excludeUUS,
            bool convertToUSD,
            string game)
        {
            var request = new GetCascadingSlotBetHistoryRequest(roundId, nickname, excludeUUS, game, convertToUSD);

            return PartialView("CascadingSlots/_CascadingSlotBetHistory", await mediator.Send(request));
        }

        [HttpGet]
        public IActionResult LasVegas() => RedirectToAction("Index", new { game = GameId.UnbalancedBigSmall1 });

        [HttpGet]
        public IActionResult MarinaBay() => RedirectToAction("Index", new { game = GameId.UnbalancedBigSmall2 });

        [HttpGet]
        public IActionResult Macau() => RedirectToAction("Index", new { game = GameId.UnbalancedBigSmall3 });

        [HttpGet]
        public IActionResult MonteCarlo() => RedirectToAction("Index", new { game = GameId.UnbalancedBigSmall4 });

        [HttpGet]
        public async Task<IActionResult> GetUnbalancedBigSmallBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_UnbalancedBigSmallBetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        [HttpGet]
        public async Task<IActionResult> GetWinThreeCardsBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_WinThreeCardsBetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        [HttpGet]
        public async Task<IActionResult> GetSlotRNGBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_SlotRNGBetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        [HttpGet]
        public async Task<IActionResult> GetFanTanBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_FanTanBetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        private async Task<ReportViewModel> GetReportViewModel()
        {
            var selections = await mediator.Send(new GetSelectionsRequest());
            var games = Game.GetNormalGames().Select(x => new SelectListItem(x.DisplayName, x.GameId.ToString()));
            return new ReportViewModel { ExcludeUUS = true, Selections = selections, Games = games };
        }

        [HttpGet]
        public async Task<IActionResult> GetDragonTigerFightBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_DragonTigerFightBetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        [HttpGet]
        public async Task<IActionResult> GetColorGameBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_ColorGameBetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        [HttpGet]
        public async Task<IActionResult> GetBandarQiuQiuBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_BandarQiuQiuBetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        [HttpGet]
        public async Task<IActionResult> GetBacBall1BetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_BacBall1BetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        [HttpGet]
        public async Task<IActionResult> GetBacBall2BetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_BacBall2BetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        [HttpGet]
        public async Task<IActionResult> CrashGameReport(string game)
        {
            var selections = await mediator.Send(new GetSelectionsRequest());
            var games = await generalGameSettingService.GetAllGames();
            var gameSetting = games.FirstOrDefault(gameInfo => gameInfo.Name == game);

            var viewModel = new CrashGameReportViewModel
            {
                ExcludeUUS = true,
                Selections = selections,
                BaseCurrency = systemSettings.BaseCurrency,
                SystemType = systemSettings.System,
                GameConfigName = game,
                GameDisplayName = gameSetting?.DisplayName,
                GameId = gameSetting.GameId
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetCrashGameReport(
           string partnerId,
           long roundId,
           string nickname,
           string username,
           bool convertToUSD,
           bool excludeUUS,
           string game)
        {
            var request = new GetCrashGameReportRequest(
                roundId,
                nickname,
                excludeUUS,
                game,
                partnerId,
                username,
                convertToUSD);

            return PartialView("CrashGames/_CrashGameRecords", await mediator.Send(request));
        }

        [HttpGet]
        public async Task<IActionResult> CrashTurnGameReport(string game)
        {
            var selections = await mediator.Send(new GetSelectionsRequest());
            var games = await generalGameSettingService.GetAllGames();
            var gameSetting = games.FirstOrDefault(gameInfo => gameInfo.Name == game);

            var viewModel = new TurnGameReportViewModel
            {
                ExcludeUUS = true,
                Selections = selections,
                BaseCurrency = systemSettings.BaseCurrency,
                SystemType = systemSettings.System,
                GameConfigName = game,
                GameDisplayName = gameSetting?.DisplayName,
                GameId = gameSetting.GameId,
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetCrashTurnGameReport(
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
            var request = new GetCrashTurnGameReportRequest(
                roundId,
                fromDate,
                toDate,
                nickname,
                excludeUUS,
                game,
                partnerId,
                username,
                convertToUSD);

            return PartialView("CrashTurnGames/_CrashTurnGameRecords", await mediator.Send(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetNextBallBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
            return PartialView("_NextBallBetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        [HttpGet]
        public async Task<IActionResult> GetFootballDiceBetHistory(BetHistoryFilter filter, bool convertToUSD)
        {
                return PartialView("_FootballDiceBetHistory", await mediator.Send(new BetHistoryReportRequest(filter, convertToUSD)));
        }

        [HttpGet]
        [ActionName("7up7down")]
        public IActionResult SevenUpDown() => RedirectToAction("Index", new { game = GameId.SevenUpDown });
    }
}