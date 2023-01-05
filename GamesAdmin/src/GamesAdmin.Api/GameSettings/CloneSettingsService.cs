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
        Task<bool> CloneNewGame(byte gameId, byte fromGameId, string displayName);
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

        public async Task<bool> CloneNewGame(byte gameId, byte fromGameId, string displayName)
        {
            try
            {
                var gameName = await generalGameSettingService.GetGameName(fromGameId);

                if (gameName != null)
                {
                    var gameEntity = await repository.FindOneAsync(entity => entity.Name == gameName);

                    gameEntity.DisplayName = displayName;
                    gameEntity.GameId = gameId;
                    gameEntity.Enabled = false;
                    gameEntity.Name = NamingConverter.GameConfigName(displayName);

                    await repository.InsertOneAsync(gameEntity);

                    return true;
                }
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }
    }
}