using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.GameUser.Results;
using Refit;

namespace GamesAdmin.Site.Features.GameUser
{
    public interface IAccountApi : IBaseAuthorizationApi
    {
        [Get("/game_users/bot/count")]
        Task<int> BotCount();

        [Post("/game_users/bots/generate")]
        Task<List<AddUsersResult>> AddBots(string[] names);

        [Post("/game_users/generate")]
        Task<List<AddUsersResult>> AddUsers(string[] names);

        [Post("/game_users/bot/revise")]
        Task<int> ReviseBots();
    }

    public interface IAccountService
    {
        Task<int> GetBotCount();

        Task<List<AddUsersResult>> AddUsers(string[] names, bool isBot);

        Task<int> ReviseBots();
    }

    public class GameUserService : IAccountService
    {
        private readonly IAccountApi accountApi;

        public GameUserService(IAccountApi accountApi)
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