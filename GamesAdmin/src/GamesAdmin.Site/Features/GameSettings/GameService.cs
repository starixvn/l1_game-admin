using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using Refit;
using Sentry;

namespace GamesAdmin.Site.Features.GameSettings
{
    public interface IGameApi
    {
        [Get("/game-settings")]
        Task<IList<GameConfig>> GetGames();

        [Post("/game-settings/")]
        Task<bool> Add([Body] GameConfig game);

        [Put("/game-settings/")]
        Task<bool> Update([Body] GameConfig game);

        [Post("/game-settings/{id}/delete")]
        Task<bool> Delete(string id);

        [Get("/game-settings/{name}")]
        Task<GameConfig> GetByName(string name);
    }

    public interface IGameService
    {
        Task<IList<GameConfig>> GetGames();

        Task<bool> Update(GameConfig game);

        Task<bool> Add(GameConfig game);

        Task<bool> Delete(string id);

        Task<GameConfig> GetByName(string name);

        Task<bool> IsExistName(string name);
    }

    public class GameService : IGameService
    {
        private readonly IGameApi gameApi;
        private readonly ISentryClient sentryClient;

        public GameService(HttpClient httpClient, ISentryClient sentryClient)
        {
            gameApi = RestService.For<IGameApi>(httpClient);
            this.sentryClient = sentryClient;
        }

        public async Task<bool> Add(GameConfig game)
        => await gameApi.Add(game);

        public async Task<IList<GameConfig>> GetGames()
        {
            try
            {
                return await gameApi.GetGames();
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
    }
}