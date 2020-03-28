using GamesAdmin.Api.Users.Results;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Users
{
    public interface IUserService
    {
        Task AddBotBalance();

        Task<int> GetTotalBots();

        Task<IList<GenerateBotResult>> GenerateUsers(string[] names, bool isBot);
    }

    public class UserService : IUserService
    {
        private const int MinAmount = 1000;
        private const int AddedAmount = 5000;

        private readonly IGenericRepository<AccountInfoEntity> accountInfoRepository;
        private readonly IGenericRepository<AccountEntity> accountRepository;
        private readonly IGenericRepository<MiniSicboAccountEntity> txAccountRepository;
        private readonly IGenericRepository<XocDiaAccountEntity> xdAccountRepository;
        private readonly IGenericRepository<BauCuaAccountEntity> bcAccountRepository;

        private readonly ISentryClient sentryClient;

        public UserService(
            ISentryClient sentryClient,
            IGenericRepository<AccountInfoEntity> accountInfoRepository,
            IGenericRepository<AccountEntity> accountRepository,
             IGenericRepository<MiniSicboAccountEntity> txAccountRepository,
            IGenericRepository<XocDiaAccountEntity> xdAccountRepository,
            IGenericRepository<BauCuaAccountEntity> bcAccountRepository)
        {
            this.sentryClient = sentryClient;
            this.accountInfoRepository = accountInfoRepository;
            this.accountRepository = accountRepository;
            this.txAccountRepository = txAccountRepository;
            this.xdAccountRepository = xdAccountRepository;
            this.bcAccountRepository = bcAccountRepository;
        }

        public Task AddBotBalance()
        => Task.Run(() =>
        {
            var botsNeedAddBalance = accountInfoRepository.FilterBy(entity => entity.isBot && entity.red < MinAmount).ToList();

            sentryClient.CaptureMessage($"AddBalance for {botsNeedAddBalance?.Count}");

            foreach (var bot in botsNeedAddBalance)
            {
                bot.red += AddedAmount;

                accountInfoRepository.ReplaceOne(bot);
            }
        });

        public Task<int> GetTotalBots()
        => Task.Run(() =>
        {
            return accountInfoRepository.FilterBy(entity => entity.isBot).Count();
        });

        public async Task<IList<GenerateBotResult>> GenerateUsers(string[] names, bool isBot)
        {
            var results = new List<GenerateBotResult>();

            // get last UserInfo.UID to make it increase
            var latestUserInfoUID = accountInfoRepository.FilterBy(userInfo => userInfo.UID > 0).LastOrDefault()?.UID ?? 0;

            foreach (var name in names)
            {

                latestUserInfoUID += 1;
                var userName = $"{name}-{(new Random()).Next(100)}";
                var avatarId = new Random().Next(1, 40);
                var result = new GenerateBotResult(userName, name, avatarId);
                
                try
                {
                    var password = $"pawd-{(new Random()).Next(200, 1000)}-{new Random().Next(150, 500)}";
                    var hashPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));

                    var account = new AccountEntity
                    {
                        local = new LocalAccount
                        {
                            username = userName,
                            password = hashPassword,
                            regDate = DateTime.UtcNow
                        }
                    };

                    await accountRepository.InsertOneAsync(account);

                    var createdAccountId = accountRepository
                        .FilterBy(account => account.local.username == userName)
                        .FirstOrDefault()
                        ?.id.ToString();

                    if (!string.IsNullOrWhiteSpace(createdAccountId))
                    {
                        var accountInfo = new AccountInfoEntity
                        {
                            name = name,
                            avatarId = avatarId,
                            red = 10000,
                            isBot = isBot,
                            userId = createdAccountId,
                            joinedOn = DateTime.UtcNow,
                            UID = latestUserInfoUID
                        };

                        await accountInfoRepository.InsertOneAsync(accountInfo);

                        await txAccountRepository.InsertOneAsync(new MiniSicboAccountEntity { uid = createdAccountId });
                        await xdAccountRepository.InsertOneAsync(new XocDiaAccountEntity { uid = createdAccountId });
                        await bcAccountRepository.InsertOneAsync(new BauCuaAccountEntity { uid = createdAccountId });
                    }
                }
                catch (Exception ex)
                {
                    sentryClient.CaptureException(ex);
                    result.Error = ex.Message;
                }

                results.Add(result);
            }

            return results.ToArray();
        }
    }
}
