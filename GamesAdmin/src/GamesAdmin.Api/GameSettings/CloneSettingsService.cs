using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        Task<string> CloneNewGame(int gameId, int fromGameId, string displayName);
    }

    public class CloneSettingsService : ICloneSettingService
    {
        private readonly IGenericRepository<GameConfigEntity> repository;
        private readonly IGeneralGameSettingService generalGameSettingService;
        private readonly ISentryClient sentryClient;
        private readonly INotificationService notificationService;
        private readonly IGameSettingsService gameService;

        public CloneSettingsService(IGenericRepository<GameConfigEntity> repository,
            IGeneralGameSettingService generalGameSettingService,
            ISentryClient sentryClient,
            INotificationService notificationService,
            IGameSettingsService gameService)
        {
            this.repository = repository;
            this.generalGameSettingService = generalGameSettingService;
            this.sentryClient = sentryClient;
            this.notificationService = notificationService;
            this.gameService = gameService;
        }

        public async Task<string> CloneNewGame(int gameId, int fromGameId, string displayName)
        {
            try
            {        
                var gameName = await generalGameSettingService.GetGameName(fromGameId);
                var existingGame = await repository.FindOneAsync(entity => entity.Name ==  NamingConverter.GameConfigName(displayName) || entity.GameId == gameId);
               
                if (existingGame != null)
                {
                    return "GameId or GameName is existed !";
                }

                if (fromGameId == 0)
                {
                    var createResult = await CreateNewCrashGame(displayName, gameId);
                    return createResult ? string.Empty : "Clone game failed!";

                }

                var notification = await notificationService.GetByGameName(gameName, NotificationDisplayKind.DisabledGame);

                if (notification != null)
                {
                    await notificationService.AddDisableMessage(NamingConverter.GameConfigName(displayName), notification.Messages);
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

        public async Task<bool> CreateNewCrashGame( string displayName, int gameId)
        {
            var gameInfo = new GameInfo(NamingConverter.GameConfigName(displayName), false, GameType.Crash, 1, displayName, gameId, null);
            var enableSetting = new EnableSetting(false);
            var botSetting = new BotSetting(false);
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


            await gameService.Create(gameConfig);

            return true;
        }
    }
}