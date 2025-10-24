using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api.CurrencyGroupGames;
using GamesAdmin.Api.Games;
using GamesAdmin.Api.GeneralGameSettings;
using GamesAdmin.Api.Notifications;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Sentry;

namespace GamesAdmin.Api.GameSettings
{
    public interface ICloneSettingService
    {
        Task<string> CloneNewGame(int gameId, int fromGameId, string displayName, byte gameType);
    }

    public class CloneSettingsService : ICloneSettingService
    {
        private readonly IGenericRepository<GameConfigEntity> repository;
        private readonly IGeneralGameSettingService generalGameSettingService;
        private readonly ISentryClient sentryClient;
        private readonly INotificationService notificationService;
        private readonly IGameSettingsService gameService;
        private readonly ICurrencyGroupGameService currencyGroupGameService;
        private readonly int BOT_MAX_BET_DEFAULT = 10;

        public CloneSettingsService(IGenericRepository<GameConfigEntity> repository,
            IGeneralGameSettingService generalGameSettingService,
            ISentryClient sentryClient,
            INotificationService notificationService,
            IGameSettingsService gameService,
            ICurrencyGroupGameService currencyGroupGameService)
        {
            this.repository = repository;
            this.generalGameSettingService = generalGameSettingService;
            this.sentryClient = sentryClient;
            this.notificationService = notificationService;
            this.gameService = gameService;
            this.currencyGroupGameService = currencyGroupGameService;
        }

        public async Task<string> CloneNewGame(int gameId, int fromGameId, string displayName, byte gameType)
        {
            try
            {
                var gameName = await generalGameSettingService.GetGameName(fromGameId);
                var existingGame = await repository.FindOneAsync(entity => entity.Name == NamingConverter.GameConfigName(displayName) || entity.GameId == gameId);
                var currencyGroupGameSettings = await currencyGroupGameService.GetByGameId(fromGameId);

                if (existingGame != null)
                {
                    return "GameId or GameName is existed !";
                }

                if (fromGameId == 0)
                {
                    var createResult = await CreateNewCrashGame(displayName, gameId, gameType);
                    return createResult ? string.Empty : "Clone game failed!";
                }

                var notification = await notificationService.GetByGameName(gameName, NotificationDisplayKind.DisabledGame);

                if (notification != null)
                {
                    await notificationService.AddDisableMessage(NamingConverter.GameConfigName(displayName), notification.Messages);
                }

                foreach (var setting in currencyGroupGameSettings)
                {
                    setting.GameId = gameId;
                    setting.GameName = NamingConverter.GameConfigName(displayName);
                    setting.Enabled = false;

                    await currencyGroupGameService.Update(setting);
                }

                if (gameName != null)
                {
                    var gameEntity = await repository.FindOneAsync(entity => entity.Name == gameName);

                    gameEntity.DisplayName = displayName;
                    gameEntity.GameId = gameId;
                    gameEntity.Enabled = false;
                    gameEntity.Name = NamingConverter.GameConfigName(displayName);

                    await repository.InsertOneAsync(gameEntity);

                    return String.Empty;
                }
                else
                {
                    return "Cloned Game is not existed !";
                }
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return ex.Message;
            }
        }

        public async Task<bool> CreateNewCrashGame(string displayName, int gameId, byte gameType)
        {
            var gameInfo = new GameInfo(NamingConverter.GameConfigName(displayName), false, (GameType)gameType, 1, displayName, gameId, null);
            var enableSetting = new EnableSetting(false);
            var botSetting = new BotSetting(false, BOT_MAX_BET_DEFAULT, new double[24]);
            var betLimit = new BetLimit();
            var gameConfig = new GameConfig(
                 string.Empty,
                 new Dictionary<string, double>(),
                 gameInfo,
                 enableSetting,
                 botSetting,
                 betLimit,
                 null,
                 null,
                 null,
                 new CrashConfig());

            if (gameType == (byte)GameType.CrashTurnBase)
            {
                gameConfig.CrashConfig.MultiplierConfigs = new MultiplierConfig
                {
                    InitialMultiplier = 1.01,
                    Stages = new List<MultiplierStage> { new MultiplierStage(), new MultiplierStage(), new MultiplierStage(), new MultiplierStage() }
                };
            }

            if (gameType == (byte)GameType.FixedStepsCrashTurnBase)
            {
                gameConfig.CrashConfig.FixedStepsMultiplierConfigs = new List<FixedStepsMultiplierConfig> {
                    new FixedStepsMultiplierConfig { Mode = (byte)Core.Enumeration.CrashGame.GameMode.Easy, MultiplierSteps = new List<double> { }},
                    new FixedStepsMultiplierConfig { Mode = (byte)Core.Enumeration.CrashGame.GameMode.Medium, MultiplierSteps = new List<double> { }},
                    new FixedStepsMultiplierConfig { Mode = (byte)Core.Enumeration.CrashGame.GameMode.Hard, MultiplierSteps = new List<double> { }},
                    new FixedStepsMultiplierConfig { Mode = (byte)Core.Enumeration.CrashGame.GameMode.HardCore, MultiplierSteps = new List<double> { }}
                };
            }

            await gameService.Create(gameConfig);

            return true;
        }
    }
}