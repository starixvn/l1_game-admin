using System.Threading.Tasks;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.Bot
{
    public interface IBotApi : IBaseAuthorizationApi
    {
        [Post("/accounts/bot/revise_balance")]
        Task<int> ReviseBalance();

        [Post("/accounts/bot/disable_randomly")]
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

        public Task DisableRandomly() => api.ReviseBalance();

        public Task ReviseBalance() => api.DisableRandomly();
    }
}
