using GamesAdmin.Site.Features.Account.Results;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Account
{
    public interface IAccountApi
    {
        [Get("/users/bot/count")]
        Task<int> BotCount();

        [Post("/users/bots/generate")]
        Task<List<AddUsersResult>> AddBots(string[] names);

        [Post("/users/generate")]
        Task<List<AddUsersResult>> AddUsers(string[] names);
    }

    public interface IAccountService
    {
        Task<int> GetBotCount();

        Task<List<AddUsersResult>> AddUsers(string[] names, bool isBot);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountApi accountApi;

        public AccountService(IAccountApi accountApi)
        {
            this.accountApi = accountApi;
        }

        public Task<List<AddUsersResult>> AddUsers(string[] names, bool isBot)
        => isBot ? accountApi.AddBots(names) : accountApi.AddUsers(names);

        public Task<int> GetBotCount()
        => accountApi.BotCount();
    }
}
