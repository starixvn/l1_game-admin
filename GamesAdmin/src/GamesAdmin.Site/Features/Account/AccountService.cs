using GamesAdmin.Site._Shared.IdentityServer;
using GamesAdmin.Site.Features.Account.Results;
using IdentityModel.Client;
using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Account
{
    public interface IAccountApi
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

        public AccountService(HttpClient httpClient)
        {
            accountApi = RestService.For<IAccountApi>(httpClient);
        }

        public async Task<List<AddUsersResult>> AddUsers(string[] names, bool isBot)
        => isBot ? await  accountApi.AddBots(names) : await accountApi.AddUsers(names);

        public async Task<int> GetBotCount()
        => await accountApi.BotCount();

        public async Task<int> ReviseBots()
        => await accountApi.ReviseBots();
    }
}
