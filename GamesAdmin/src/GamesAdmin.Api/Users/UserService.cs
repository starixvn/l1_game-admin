using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Sentry;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Users
{
    public interface IUserService
    {
        Task AddBotBalance();
    }

    public class UserService : IUserService
    {
        private const int MinAmount = 12000;
        private const int AddedAmount = 5000;

        private readonly IGenericRepository<UserInfoEntity> repository;
        private readonly ISentryClient sentryClient;

        public UserService(IGenericRepository<UserInfoEntity> repository, ISentryClient sentryClient)
        {
            this.repository = repository;
            this.sentryClient = sentryClient;
        }

        public Task AddBotBalance()
        => Task.Run(() =>
        {
            var botsNeedAddBalance = repository.FilterBy(entity => entity.isBot && entity.red < MinAmount).ToList();

            sentryClient.CaptureMessage($"AddBalance for {botsNeedAddBalance?.Count}");

            foreach (var bot in botsNeedAddBalance)
            {
                bot.red += AddedAmount;

                repository.ReplaceOne(bot);
            }
        });
    }
}
