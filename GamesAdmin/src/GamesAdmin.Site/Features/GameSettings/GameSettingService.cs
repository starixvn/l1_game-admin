using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;
using Sentry;

namespace GamesAdmin.Site.Features.GameSettings
{
    public interface IGameSettingApi : IBaseAuthorizationApi
    {
        [Get("/game_settings")]
        Task<IList<GameConfig>> GetAll();

        [Post("/game_settings/")]
        Task<bool> Add([Body] GameConfig game);

        [Put("/game_settings/")]
        Task<bool> Update([Body] GameConfig game);

        [Post("/game_settings/{id}/delete")]
        Task<bool> Delete(string id);

        [Get("/game_settings/{name}")]
        Task<GameConfig> GetByName(string name);

        [Put("/game_settings/{name}/update_status")]
        Task<bool> UpdateStatus(string name, bool enabled);
    }

    public interface IGameSettingService
    {
        Task<IList<GameConfig>> GetAll();

        Task<bool> Update(GameConfig game);

        Task<bool> Add(GameConfig game);

        Task<bool> Delete(string id);

        Task<GameConfig> GetByName(string name);

        Task<bool> IsExistName(string name);

        Task<bool> UpdateStatus(string name, bool enabled);
    }

    public class GameSettingService : IGameSettingService
    {
        private readonly IGameSettingApi gameApi;
        private readonly ISentryClient sentryClient;

        public GameSettingService(ISentryClient sentryClient, IGameSettingApi gameApi)
        {
            this.sentryClient = sentryClient;
            this.gameApi = gameApi;
        }

        public async Task<bool> Add(GameConfig game)
        => await gameApi.Add(game);

        public async Task<IList<GameConfig>> GetAll()
        {
            try
            {
                return await gameApi.GetAll();
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return new List<GameConfig>() as IList<GameConfig>;
            }
        }

        public async Task<bool> Update(GameConfig game)
        => await gameApi.Update(game);

        public async Task<bool> Delete(string id)
        => await gameApi.Delete(id);

        public async Task<GameConfig> GetByName(string name)
        => await gameApi.GetByName(name);

        public async Task<bool> IsExistName(string name)
        {
            var gameConfig = await GetByName(name);

            return gameConfig != default;
        }

        public Task<bool> UpdateStatus(string name, bool enabled)
        => gameApi.UpdateStatus(name, enabled);
    }
}