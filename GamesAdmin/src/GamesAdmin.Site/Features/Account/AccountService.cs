using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Account.Results;
using Refit;

namespace GamesAdmin.Site.Features.Account
{
    public interface IAccountApi : IBaseAuthorizationApi
    {
        [Get("/accounts/bot/count")]
        Task<int> BotCount();

        [Post("/accounts/bots/generate")]
        Task<List<AddUsersResult>> AddBots(string[] names);

        [Post("/accounts/generate")]
        Task<List<AddUsersResult>> AddUsers(string[] names);

        [Post("/accounts/bot/revise")]
        Task<int> ReviseBots();
    }

    public interface IAccountService
    {
        Task<int> GetBotCount();

        Task<List<AddUsersResult>> AddUsers(string[] names, bool isBot);

        Task<int> ReviseBots();
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountApi accountApi;

        public AccountService(IAccountApi accountApi)
        {
            this.accountApi = accountApi;
        }

        public async Task<List<AddUsersResult>> AddUsers(string[] names, bool isBot)
        => isBot ? await accountApi.AddBots(names) : await accountApi.AddUsers(names);

        public async Task<int> GetBotCount()
        => await accountApi.BotCount();

        public async Task<int> ReviseBots()
        => await accountApi.ReviseBots();
    }
}