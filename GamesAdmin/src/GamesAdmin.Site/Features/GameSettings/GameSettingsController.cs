using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Extensions;
using GamesAdmin.Site.Features.GameSettings.Requests;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sentry;

namespace GamesAdmin.Site.Features.GameSettings
{
    [Authorize]
    public class GameSettingsController : Controller
    {
        private readonly IMediator mediator;
        private readonly ISentryClient sentryClient;
        private readonly IAppSettings appSettings;
        private readonly IMapper mapper;

        public GameSettingsController(IMediator mediator, ISentryClient sentryClient, IAppSettings appSettings, IMapper mapper)
        {
            this.mediator = mediator;
            this.sentryClient = sentryClient;
            this.appSettings = appSettings;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        => View(await mediator.Send(new AddRequest()));

        [HttpPost]
        public async Task<IActionResult> Add([Bind] AddViewModel addViewModel)
        {
            await mediator.Send(new CreateRequest(
                mapper.Map<GameInfo>(addViewModel),
                mapper.Map<EnableSetting>(addViewModel),
                mapper.Map<BotSetting>(addViewModel),
                mapper.Map<BetLimit>(addViewModel),
                mapper.Map<SlotsConfig>(addViewModel).GenerateDefaultSlotIcons(addViewModel.NumberOfIcons, addViewModel.OddsPerIcon)));

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("{name}")]
        public async Task<IActionResult> UpdateBotRatio(string name, [Bind] List<BotRatioItemViewModel> botRatioItems)
        {
            var botRatios = botRatioItems
                .OrderBy(item => item.UtcHour)
                .Select(item => item.BotRatio / 100)
                .ToArray();

            await mediator.Send(new UpdateBotRatioRequest(name, botRatios));

            TempData["isSuccess"] = true;

            return Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
            => View("~/Features/GameSettings/Views/GameManagement.cshtml", await mediator.Send(new GetStatusRequest()));

        [HttpGet]
        public async Task<IActionResult> GameMonitor()
            => View(await mediator.Send(new GetMonitorRequest()));

        [HttpGet]
        [Route("gamedetails/{name}")]
        public async Task<IActionResult> GameDetails(string name)
        {
            var viewModel = await mediator.Send(new EditRequest(name));
            viewModel.ErrorMessage = TempData["errorMessage"]?.ToString();
            viewModel.Success = TempData["isSuccess"] != null && (bool)TempData["isSuccess"];

            return View(viewModel);
        }

        [HttpGet]
        [Route("botratio/{gameName}")]
        public async Task<IActionResult> BotRatio(string gameName)
        {
            var gameSetting = await mediator.Send(new EditRequest(gameName));

            return Json(gameSetting);
        }

        [HttpPost]
        [Route("status/enable")]
        public async Task<IActionResult> EnableGames(string[] games, bool reload = false)
        {
            var result = await mediator.Send(new UpdateMultipleGameStatusRequest(games, true, reload));

            return Json(result);
        }

        [HttpPost]
        [Route("status/disable")]
        public async Task<IActionResult> DisableGames(string[] games, bool reload = false)
        {
            var result = await mediator.Send(new UpdateMultipleGameStatusRequest(games, false, reload));

            return Json(result);
        }

        [HttpPost]
        [Route("status")]
        public async Task<IActionResult> UpdateDisabledMessage(string name, string vnMessage, string enMessage)
        {
            if (string.IsNullOrWhiteSpace(vnMessage))
            {
                TempData["errorMessage"] = "Message cannot be empty.";

                return Json(false);
            }
            else
            {
                var messages = new Dictionary<string, string>
                {
                    {Language.VnValue, vnMessage },
                    { Language.EnValue, enMessage }
                };

                await mediator.Send(new UpdateDisabledMessageRequest(name, messages));

                return Json(true);
            }
        }

        [HttpGet]
        [Route("status/disableMessage")]
        public async Task<IActionResult> GetDisabledMessage(string name)
        {
            var disabledMessages = await mediator.Send(new GetDisabledMessageRequest(name));

            return Json(disabledMessages);
        }        

        [HttpPost]
        [Route("status/reload")]
        public async Task<IActionResult> ReloadGameClients()
        {
            var result = await mediator.Send(new ReloadGameClientRequest());

            return Json(result);
        }

        [Route("status/um")]
        public IActionResult UnderMaintenance()
        => View(new UnderMaintenanceViewModel(appSettings));

        [HttpPost]
        [Route("status/um")]
        public async Task<IActionResult> UnderMaintenance([Bind] UnderMaintenanceRequest request)
        {
            var result = await mediator.Send(request);

            return Json(result);
        }

        [HttpPost]
        [Route("/jwt")]
        public async Task<IActionResult> JWT([FromBody] GetJWTRequest request)
        {
            request.Username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;

            var result = await mediator.Send(request);

            HttpContext.Response.Cookies.Append("token", result.Token);
            HttpContext.Response.Cookies.Append("game-server-endpoint", appSettings.GameServers.Main.Socket);
            HttpContext.Response.Cookies.Append("sedie-server-endpoint", appSettings.GameServers.Sedie.Socket);
            HttpContext.Response.Cookies.Append("sedie-server-endpoint-route", appSettings.GameServers.Sedie.SocketRoute);
            HttpContext.Response.Cookies.Append("vip-sedie-server-endpoint", appSettings.GameServers.VipSedie.Socket);
            HttpContext.Response.Cookies.Append("vip-sedie-server-endpoint-route", appSettings.GameServers.VipSedie.SocketRoute);
            HttpContext.Response.Cookies.Append("special-sedie-server-endpoint", appSettings.GameServers.SpecialSedie.Socket);
            HttpContext.Response.Cookies.Append("special-sedie-server-endpoint-route", appSettings.GameServers.SpecialSedie.SocketRoute);
            HttpContext.Response.Cookies.Append("vip-fish-prawn-crab-server-endpoint", appSettings.GameServers.VipFishPrawnCrab.Socket);
            HttpContext.Response.Cookies.Append("vip-fish-prawn-crab-server-endpoint-route", appSettings.GameServers.VipFishPrawnCrab.SocketRoute);
            HttpContext.Response.Cookies.Append("sic-bo-server-endpoint", appSettings.GameServers.SicBo.Socket);
            HttpContext.Response.Cookies.Append("sic-bo-server-endpoint-route", appSettings.GameServers.SicBo.SocketRoute);
            HttpContext.Response.Cookies.Append("vip-baccarat-server-endpoint", appSettings.GameServers.VipBaccarat.Socket);
            HttpContext.Response.Cookies.Append("vip-baccarat-server-endpoint-route", appSettings.GameServers.VipBaccarat.SocketRoute);
            HttpContext.Response.Cookies.Append("vip-sedie-war-server-endpoint", appSettings.GameServers.VipSeDieWar.Socket);
            HttpContext.Response.Cookies.Append("vip-sedie-war-server-endpoint-route", appSettings.GameServers.VipSeDieWar.SocketRoute);
            HttpContext.Response.Cookies.Append("keno-pro-max-server-endpoint", appSettings.GameServers.KenoProMax.Socket);
            HttpContext.Response.Cookies.Append("keno-pro-max-server-endpoint-route", appSettings.GameServers.KenoProMax.SocketRoute);
            HttpContext.Response.Cookies.Append("mini-sedie-server-endpoint", appSettings.GameServers.MiniSeDie.Socket);
            HttpContext.Response.Cookies.Append("mini-sedie-server-endpoint-route", appSettings.GameServers.MiniSeDie.SocketRoute);
            HttpContext.Response.Cookies.Append("fish-prawn-crab-server-endpoint", appSettings.GameServers.FishPrawnCrab.Socket);
            HttpContext.Response.Cookies.Append("fish-prawn-crab-server-endpoint-route", appSettings.GameServers.FishPrawnCrab.SocketRoute);
            HttpContext.Response.Cookies.Append("big-small-endpoint", appSettings.GameServers.BigSmall.Socket);
            HttpContext.Response.Cookies.Append("big-small-server-endpoint-route", appSettings.GameServers.BigSmall.SocketRoute);
            HttpContext.Response.Cookies.Append("turbo-big-small-endpoint", appSettings.GameServers.TurboBigSmall.Socket);
            HttpContext.Response.Cookies.Append("turbo-big-small-server-endpoint-route", appSettings.GameServers.TurboBigSmall.SocketRoute);
            HttpContext.Response.Cookies.Append("dragon-tiger-endpoint", appSettings.GameServers.DragonTiger.Socket);
            HttpContext.Response.Cookies.Append("dragon-tiger-server-endpoint-route", appSettings.GameServers.DragonTiger.SocketRoute);
            HttpContext.Response.Cookies.Append("turbo-odd-even-endpoint", appSettings.GameServers.TurboOddEven.Socket);
            HttpContext.Response.Cookies.Append("turbo-odd-even-server-endpoint-route", appSettings.GameServers.TurboOddEven.SocketRoute);
            HttpContext.Response.Cookies.Append("roulette-endpoint", appSettings.GameServers.Roulette.Socket);
            HttpContext.Response.Cookies.Append("roulette-server-endpoint-route", appSettings.GameServers.Roulette.SocketRoute);
            HttpContext.Response.Cookies.Append("baccarat-server-endpoint", appSettings.GameServers.Baccarat.Socket);
            HttpContext.Response.Cookies.Append("baccarat-server-endpoint-route", appSettings.GameServers.Baccarat.SocketRoute);
            HttpContext.Response.Cookies.Append("lottery-server-endpoint", appSettings.GameServers.Lottery.Socket);
            HttpContext.Response.Cookies.Append("lottery-server-endpoint-route", appSettings.GameServers.Lottery.SocketRoute);

            HttpContext.Response.Cookies.Append("env", result.Env);

            return Json(result);
        }

        [HttpPost]
        [Route("/payouts/update")]
        public async Task<IActionResult> UpdatePayouts([Bind] EditViewModel viewModel)
        {
            try
            {
                await mediator.Send(new UpdateBetLimitAndOddsRequest(
                    viewModel.Name,
                    viewModel.Type,
                    new BetLimit(viewModel.PayoutViewModel.MinBet, viewModel.PayoutViewModel.MaxBet, viewModel.PayoutViewModel.MaxBetChoices),
                    viewModel.PayoutViewModel.Odds,
                    viewModel.PayoutViewModel.Icons));

                return Json(true);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return Json(false);
        }

        [HttpPost]
        [Route("/jackpot/update")]
        public async Task<IActionResult> UpdateJackpot([Bind] EditViewModel viewModel)
        {
            try
            {
                await mediator.Send(new UpdateJackpotRequest(
                    viewModel.Name,
                    new Jackpot(
                        viewModel.JackpotViewModel.JackpotEnabled,
                        viewModel.JackpotViewModel.WinningRatio.ConvertPercentValue(),
                        viewModel.JackpotViewModel.PlayerContribution.ConvertPercentValue(),
                        viewModel.JackpotViewModel.BotContribution.ConvertPercentValue(),
                        viewModel.JackpotViewModel.ExcludeUUS),
                    viewModel.JackpotViewModel.StakeLevels));

                return Json(true);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return Json(false);
        }

        [HttpPost]
        [Route("/update")]
        public async Task<IActionResult> Update([Bind] EditViewModel viewModel)
        {
            try
            {
                await mediator.Send(new UpdateRequest(
                    new GameInfo(viewModel.Name, viewModel.EnableFreeBet, viewModel.Type),
                    new BotSetting(viewModel.BotEnabled, viewModel.BotMaxBet, null, viewModel.SmartBotEnabled, viewModel.SmartBotGroupRatio.ConvertPercentValue()),
                    new EnableSetting(viewModel.Enabled, viewModel.DelayStartTime),
                    viewModel.SlotViewModel == null ? new SlotsConfig() : new SlotsConfig(
                        (byte)viewModel.SlotViewModel.Mode,
                        viewModel.SlotViewModel.BigWinRatio.ConvertPercentValue(),
                        new Bonus(viewModel.SlotViewModel.MinBonus, viewModel.SlotViewModel.MaxBonus))));

                return Json(true);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return Json(false);
        }

        [HttpPost]
        [Route("exclusions/update")]
        public async Task<IActionResult> UpdateSiteExclusions([Bind] UpdateSiteExclusionRequest request)
        {
            try
            {
                await mediator.Send(request);

                return Json(true);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return Json(false);
        }
    }
}