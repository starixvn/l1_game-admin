using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Audit.Mvc;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Extensions;
using GamesAdmin.Site.Features.GameSettings.Requests;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
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
        public async Task<IActionResult> CreateNewGame()
       => View(await mediator.Send(new CreateNewGameRequest()));

        [HttpGet]
        [Route("games")]
        public async Task<IEnumerable<SelectListItem>> GetGamesByType(GameType type)
        => (await mediator.Send(new GetGamesByTypeRequest(type)));

        [HttpPost, Audit]
        public async Task<string> CloneSlotGame([Bind] AddSlotGameViewModel addViewModel)
        {
            var request = new CloneSlotGameRequest(addViewModel.DisplayName,
                addViewModel.GameId,
                addViewModel.SelectedGame,
                (byte)addViewModel.GameType);

            return await mediator.Send(request);
        }

        [HttpPost]
        public async Task<IActionResult> AddGameSettings([Bind] AddViewModel addViewModel)
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

        [HttpPost, Audit]
        [Route("{name}")]
        public async Task<IActionResult> UpdateBotRatio(string name, List<BotRatioItemViewModel> botRatioItems, IEnumerable<int> otherGameIdsInGroup)
        {
            var botRatios = botRatioItems
                .OrderBy(item => item.UtcHour)
                .Select(item => item.BotRatio / 100)
                .ToArray();

            await mediator.Send(new UpdateBotRatioRequest(name, botRatios, otherGameIdsInGroup));

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

        [HttpPost, Audit]
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

        [HttpPost, Audit]
        [Route("status/reload")]
        public async Task<IActionResult> ReloadGameClients()
        {
            var result = await mediator.Send(new ReloadGameClientRequest());

            return Json(result);
        }

        [Route("status/um")]
        public IActionResult UnderMaintenance()
        => View(new UnderMaintenanceViewModel(appSettings));

        [HttpPost, Audit]
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
                    viewModel.GameId,
                    viewModel.Name,
                    viewModel.Type,
                    new BetLimit(viewModel.PayoutViewModel.MinBet, viewModel.PayoutViewModel.MaxBet, viewModel.PayoutViewModel.MaxBetChoices),
                    viewModel.OtherGameIdsInGroup
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
                      viewModel.CascadingSlotsConfig,
                      viewModel.OtherGameIdsInGroup));

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
                await mediator.Send(new UpdateJackpotRequest(viewModel.Name, mapper.Map<Jackpot>(viewModel.JackpotViewModel), viewModel.JackpotViewModel.StakeLevels));

                return Json(true);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return Json(false);
        }

        [HttpPost, Audit]
        [Route("/slotBot/update")]
        public async Task<IActionResult> UpdateSlotBotConfig([Bind] EditViewModel viewModel)
        {
            try
            {
                await mediator.Send(new UpdateSlotBotConfigRequest(viewModel.Name, mapper.Map<Jackpot>(viewModel.JackpotViewModel), viewModel.SlotsConfig.BetLevels));

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
                CrashConfig crashConfig = null;
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

                    case GameType.Crash:
                        crashConfig = viewModel.CrashConfig;
                        break;

                    default: break;
                }

                var videoStreamConfig = viewModel.VideoStreamConfig;

                if (!string.IsNullOrEmpty(viewModel.VideoStreamConfigJson))
                {
                    if (viewModel.VideoStreamConfig == null)
                    {
                        viewModel.VideoStreamConfig = new VideoStreamConfig();
                    }

                    viewModel.VideoStreamConfig.PopulateFromJson(viewModel.VideoStreamConfigJson);
                }

                var request = new UpdateRequest(
                    new GameInfo(viewModel.Name, viewModel.EnableFreeBet, viewModel.Type, viewModel.RandomMode, string.Empty, viewModel.GameId, null, viewModel.MaxWinPerRound),
                    new BotSetting(viewModel.BotEnabled, viewModel.BotMaxBet, null, viewModel.SmartBotEnabled, viewModel.SmartBotGroupRatio.ConvertPercentValue()),
                    new EnableSetting(viewModel.Enabled, viewModel.DelayStartTime),
                    slotsConfig,
                    cascadingSlotsConfig,
                    slotRNGConfig,
                    crashConfig,
                    viewModel.OddsMultipliers,
                    viewModel.TournamentConfig,
                    viewModel.VideoStreamConfig,
                    viewModel.OtherGameIdsInGroup);

                await mediator.Send(request);

                return Json(true);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return Json(false);
        }

        [HttpPost]
        [Route("serviceStatus/check")]
        public async Task<IActionResult> CheckServiceStatus(string username, string password, string name)
        {
            try
            {
                var result = await mediator.Send(new CheckServiceStatusRequest(new Player(username, password), name));

                return Json(result);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return Json(false);
        }

        [HttpPost, Audit]
        [Route("/symbolweight/update")]
        public async Task<IActionResult> UpdateSymbolWeight(string gameName, byte mode, int[][] weights)
        {
            try
            {
                await mediator.Send(new UpdateSymbolWeightByModeRequest(
                    gameName,
                    mode,
                    weights));

                return Json(true);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return Json(false);
        }

        [HttpPost]
        [Route("/symbolWeight/checkRtp")]
        public async Task<IActionResult> GetSlotRtp(string odds, int[][] weight, int numOfRounds)
        {
            try
            {
                var deserializeOdds = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, double>>>(odds);
                var result = await mediator.Send(new GetSlotRtpRequest(deserializeOdds, weight, numOfRounds));

                return Json(result);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return Json(false);
        }

        [HttpGet]
        [Route("/margin/create-setting")]
        public IActionResult CreateMarginSetting(string oddsNames)
        {
            var marginSettings = oddsNames.Split(",").ToDictionary(name => name, _ => (double)0);
            var viewModel = new MarginSettingViewModel(marginSettings);
            return PartialView("_MarginItem", viewModel);
        }

        [HttpPost]
        [Route("/margin/update")]
        public async Task<IActionResult> UpdateMarginSettings(string gameName, IEnumerable<Dictionary<string, double>> marginSettings)
        {
            try
            {
                var updateResult = await mediator.Send(new UpdateMarginSettingsRequest(gameName, marginSettings));
                return Json(updateResult);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return Json(false);
        }
    }
}