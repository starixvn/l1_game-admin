using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Audit.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> AddSlotGame()
        => View(await mediator.Send(new AddSlotGameRequest()));

        [HttpPost]
        public async Task<IActionResult> CloneSlotGame([Bind] AddSlotGameViewModel addViewModel)
        {
            var request = new CloneSlotGameRequest(addViewModel.DisplayName,
                addViewModel.GameId,
                addViewModel.SelectedGame);

            await mediator.Send(request);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Add([Bind] AddViewModel addViewModel)
        {
            var request = new CreateRequest(
                mapper.Map<GameInfo>(addViewModel),
                mapper.Map<EnableSetting>(addViewModel),
                mapper.Map<BotSetting>(addViewModel),
                mapper.Map<BetLimit>(addViewModel),
                mapper.Map<SlotsConfig>(addViewModel).GenerateDefaultSlotInfo(addViewModel.NumberOfIcons, addViewModel.OddsPerIcon),
                mapper.Map<CascadingSlotsConfig>(addViewModel).GenerateDefaultCascadingSlotsConfig(addViewModel.NumberOfIcons),
                mapper.Map<SlotRNGConfig>(addViewModel));

            await mediator.Send(request);

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
            => View(await mediator.Send(new GetMonitorRequest() { Username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value }));

        [HttpGet]
        [Route("gamedetails/{name}")]
        public async Task<IActionResult> GameDetails(string name, string view)
        {
            var viewModel = await mediator.Send(new EditRequest(name));
            viewModel.ErrorMessage = TempData["errorMessage"]?.ToString();
            viewModel.Success = TempData["isSuccess"] != null && (bool)TempData["isSuccess"];

            return View(view, viewModel);
        }

        [HttpGet]
        [Route("botratio/{gameName}")]
        public async Task<IActionResult> BotRatio(string gameName)
        {
            var gameSetting = await mediator.Send(new EditRequest(gameName));

            return Json(gameSetting);
        }

        [HttpPost, Audit]
        [Route("status/enable")]
        public async Task<IActionResult> EnableGames(string[] games, bool reload = false)
        {
            var result = await mediator.Send(new UpdateMultipleGameStatusRequest(games, true, reload));

            return Json(result);
        }

        [HttpPost, Audit]
        [Route("status/disable")]
        public async Task<IActionResult> DisableGames(string[] games, bool reload = false)
        {
            var result = await mediator.Send(new UpdateMultipleGameStatusRequest(games, false, reload));

            return Json(result);
        }

        [HttpPost]
        [Route("status")]
        public async Task<IActionResult> UpdateDisabledMessage(string name, Dictionary<string, string> messages)
        {
            var defaultMessage = messages[Language.DefaultLanguageValue];
            if (string.IsNullOrWhiteSpace(defaultMessage))
            {
                TempData["errorMessage"] = "Message cannot be empty.";

                return Json(false);
            }

            await mediator.Send(new UpdateDisabledMessageRequest(name, messages));

            return Json(true);
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
            return Json(result);
        }

        [HttpPost, Audit]
        [Route("/betlimit/update")]
        public async Task<IActionResult> UpdatePayouts([Bind] EditViewModel viewModel)
        {
            try
            {
                await mediator.Send(new UpdateBetLimitRequest(
                    viewModel.Name,
                    viewModel.Type,
                    new BetLimit(viewModel.PayoutViewModel.MinBet, viewModel.PayoutViewModel.MaxBet, viewModel.PayoutViewModel.MaxBetChoices)
            ));

                return Json(true);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return Json(false);
        }

        [HttpPost, Audit]
        [Route("/odds/update")]
        public async Task<IActionResult> UpdateOdds([Bind] EditViewModel viewModel)
        {
            try
            {
                var updateResult = await mediator.Send(new UpdateOddsRequest(
                      viewModel.Name,
                      viewModel.Type,
                      viewModel.PayoutViewModel?.Odds,
                      viewModel.PayoutViewModel?.Icons,
                      viewModel.CascadingSlotsConfig));

                return Json(updateResult);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return Json(false);
        }

        [HttpPost, Audit]
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
                        viewModel.JackpotViewModel.ExcludeTestCurrency),
                    viewModel.JackpotViewModel.StakeLevels));

                return Json(true);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return Json(false);
        }

        [HttpPost, Audit]
        [Route("/update")]
        public async Task<IActionResult> Update([Bind] EditViewModel viewModel)
        {
            try
            {
                SlotsConfig slotsConfig = null;
                CascadingSlotsConfig cascadingSlotsConfig = null;
                SlotRNGConfig slotRNGConfig = null;
                switch (viewModel.Type)
                {
                    case GameType.Slots:
                        slotsConfig = new SlotsConfig(
                           (byte)viewModel.SlotViewModel.Mode,
                           viewModel.SlotViewModel.BigWinRatio.ConvertPercentValue(),
                           new Bonus(viewModel.SlotViewModel.MinBonus, viewModel.SlotViewModel.MaxBonus));
                        break;

                    case GameType.CascadingSlots:
                        cascadingSlotsConfig = new CascadingSlotsConfig(viewModel.CascadingSlotsConfig);
                        cascadingSlotsConfig.Mode = (byte)viewModel.SlotViewModel.Mode;
                        cascadingSlotsConfig.BigWinRatio = viewModel.SlotViewModel.BigWinRatio.ConvertPercentValue();
                        cascadingSlotsConfig.MultiplierBonus = new MultiplierBonus(viewModel.SlotViewModel.MinBonus, viewModel.SlotViewModel.MaxBonus);
                        break;

                    case GameType.SlotRNG:
                        slotRNGConfig = viewModel.SlotRNGConfig;
                        break;

                    default: break;
                }

                var request = new UpdateRequest(
                    new GameInfo(viewModel.Name, viewModel.EnableFreeBet, viewModel.Type, viewModel.RandomMode),
                    new BotSetting(viewModel.BotEnabled, viewModel.BotMaxBet, null, viewModel.SmartBotEnabled, viewModel.SmartBotGroupRatio.ConvertPercentValue()),
                    new EnableSetting(viewModel.Enabled, viewModel.DelayStartTime),
                    slotsConfig,
                    cascadingSlotsConfig,
                    slotRNGConfig);

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