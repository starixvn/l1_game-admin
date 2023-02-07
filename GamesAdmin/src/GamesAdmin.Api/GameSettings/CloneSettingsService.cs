using System;
using System.Threading.Tasks;
using GamesAdmin.Api.GeneralGameSettings;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Sentry;

namespace GamesAdmin.Api.GameSettings
{
    public interface ICloneSettingService
    {
        Task<string> CloneNewGame(byte gameId, byte fromGameId, string displayName);
    }

    public class CloneSettingsService : ICloneSettingService
    {
        private readonly IGenericRepository<GameConfigEntity> repository;
        private readonly IGeneralGameSettingService generalGameSettingService;
        private readonly ISentryClient sentryClient;

        public CloneSettingsService(IGenericRepository<GameConfigEntity> repository,
            IGeneralGameSettingService generalGameSettingService,
            ISentryClient sentryClient)
        {
            this.repository = repository;
            this.generalGameSettingService = generalGameSettingService;
            this.sentryClient = sentryClient;
        }

        public async Task<string> CloneNewGame(byte gameId, byte fromGameId, string displayName)
        {
            try
            {
                var gameName = await generalGameSettingService.GetGameName(fromGameId);
                var existingGame = repository.FindOneAsync(entity => entity.Name ==  NamingConverter.GameConfigName(displayName) || entity.GameId == gameId);

                if(existingGame != null)
                {
                    return "GameId or GameName is existed !";
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
    }
}