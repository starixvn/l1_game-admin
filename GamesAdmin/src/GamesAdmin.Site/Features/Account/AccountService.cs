using Refit;
using Sentry;
using System;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Account
{
    public interface IAccountApi
    {
        [Get("/users/bot/count")]
        Task<int> BotCount();
    }

    public interface IAccountService 
    {
        Task<int> GetBotCount();
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountApi accountApi;
        private readonly ISentryClient sentryClient;

        public AccountService(IAccountApi gameApi, ISentryClient sentryClient)
        {
            this.accountApi = gameApi;
            this.sentryClient = sentryClient;
        }

        public Task<int> GetBotCount()
        => accountApi.BotCount();
    }
}
