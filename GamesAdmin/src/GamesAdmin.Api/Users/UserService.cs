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
            
            var latestUserInfoUID = await GetLatestUserInfoUID();

            foreach (var name in names)
            {
                latestUserInfoUID += 1;
                var userName = $"{name}-{latestUserInfoUID}-{(new Random()).Next(100)}";
                var avatarId = new Random().Next(1, 40);
                var result = new GenerateBotResult(userName, name, avatarId);
                
                try
                {
                    var createdAccountId = await CreateAccount(userName);

                    if (!string.IsNullOrWhiteSpace(createdAccountId))
                    {
                        await CreateAccountInfo(createdAccountId, name, avatarId, isBot, latestUserInfoUID);

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

        private Task<int> GetLatestUserInfoUID()
            => Task.Run(() => { return accountInfoRepository.FilterBy(userInfo => userInfo.UID > 0).LastOrDefault()?.UID ?? 0; });

        private async Task<string> CreateAccount(string username) 
        {
            var password = $"pawd-{(new Random()).Next(200, 1000)}-{new Random().Next(150, 500)}";
            var hashPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));

            var account = new AccountEntity
            {
                local = new LocalAccount
                {
                    username = username,
                    password = hashPassword,
                    regDate = DateTime.UtcNow
                }
            };

            await accountRepository.InsertOneAsync(account);

            return accountRepository
                .FilterBy(account => account.local.username == username)
                .FirstOrDefault()
                ?.id.ToString();
        }

        private Task CreateAccountInfo(string userId, string name, int avatarId, bool isBot, int uid) 
        {
            var accountInfo = new AccountInfoEntity
            {
                name = name,
                avatarId = avatarId,
                red = 10000,
                isBot = isBot,
                userId = userId,
                joinedOn = DateTime.UtcNow,
                UID = uid
            };

            return accountInfoRepository.InsertOneAsync(accountInfo);
        }
    }
}
