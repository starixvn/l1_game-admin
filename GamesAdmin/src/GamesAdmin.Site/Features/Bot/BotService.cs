using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Bot.Results;
using Refit;

namespace GamesAdmin.Site.Features.Bot
{
    public interface IBotApi : IBaseAuthorizationApi
    {
        [Post("/game_users/bot/revise_balance")]
        Task<int> ReviseBalance();

        [Post("/game_users/bot/disable_randomly")]
        Task<int> DisableRandomly();

        [Post("/game_users/bot/deactivate")]
        Task<int> Deactivate();

        [Post("/game_users/bots/generate")]
        Task<List<AddBotResult>> Add(string[] names);
    }

    public interface IBotService
    {
        Task ReviseBalance();

        Task DisableRandomly();

        Task<int> Deactivate();

        Task<List<AddBotResult>> Add(string[] names);
    }

    public class BotService : IBotService
    {
        private readonly IBotApi api;

        public BotService(IBotApi api)
        {
            this.api = api;
        }

        public Task<List<AddBotResult>> Add(string[] names) => api.Add(names);

        public Task<int> Deactivate() => api.Deactivate();

        public Task DisableRandomly() => api.DisableRandomly();

        public Task ReviseBalance() => api.ReviseBalance();
    }
}
