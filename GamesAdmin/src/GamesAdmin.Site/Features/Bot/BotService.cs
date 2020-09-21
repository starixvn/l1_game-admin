using System.Threading.Tasks;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.Bot
{
    public interface IBotApi : IBaseAuthorizationApi
    {
        [Post("/game_users/bot/revise_balance")]
        Task<int> ReviseBalance();

        [Post("/game_users/bot/disable_randomly")]
        Task<int> DisableRandomly();
    }

    public interface IBotService
    {
        Task ReviseBalance();

        Task DisableRandomly();
    }

    public class BotService : IBotService
    {
        private readonly IBotApi api;
        public BotService(IBotApi api)
        {
            this.api = api;
        }

        public Task DisableRandomly() => api.DisableRandomly();

        public Task ReviseBalance() => api.ReviseBalance();
    }
}
