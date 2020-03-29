using Refit;
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

        public AccountService(IAccountApi gameApi)
        {
            this.accountApi = gameApi;
        }

        public Task<int> GetBotCount()
        => accountApi.BotCount();
    }
}
