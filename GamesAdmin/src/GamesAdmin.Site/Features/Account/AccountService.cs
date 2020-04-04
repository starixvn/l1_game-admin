using GamesAdmin.Site.Features.Account.Results;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Account
{
    public interface IAccountApi
    {
        [Get("/accounts/bot/count")]
        Task<int> BotCount();

        [Post("/game-settings/")]
        Task<bool> Add([Body] GameConfig game);

        [Put("/game-settings/")]
        Task<bool> Update([Body] GameConfig game);

        [Post("/game-settings/{id}/delete")]
        Task<bool> Delete(string id);
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

        public Task<List<AddUsersResult>> AddUsers(string[] names, bool isBot)
        => isBot ? accountApi.AddBots(names) : accountApi.AddUsers(names);

        public Task<int> GetBotCount()
        => accountApi.BotCount();

        public Task<int> ReviseBots()
        => accountApi.ReviseBots();
    }
}
