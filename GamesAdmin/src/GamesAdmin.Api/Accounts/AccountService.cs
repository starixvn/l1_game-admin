using GamesAdmin.Api._Shared.Helpers;
using GamesAdmin.Api.Accounts.Results;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Accounts
{
    public interface IAccountService
    {
        Task AddBotBalance();

        Task<int> GetTotalBots();

        Task<IList<GenerateAccountResult>> GenerateUsers(string[] names, bool isBot);

        Task<int> ReviseBotNames();
    }

    public class AccountService : IAccountService
    {
        private const int MinAmount = 1000;
        private const int AddedAmount = 5000;

        private readonly IGenericRepository<AccountInfoEntity> accountInfoRepository;
        private readonly IGenericRepository<AccountEntity> accountRepository;
        private readonly IGenericRepository<MiniSicboAccountEntity> txAccountRepository;
        private readonly IGenericRepository<XocDiaAccountEntity> xdAccountRepository;
        private readonly IGenericRepository<BauCuaAccountEntity> bcAccountRepository;

        private readonly ISentryClient sentryClient;

        public AccountService(
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
            var bots = accountInfoRepository.FilterBy(entity => entity.isBot);

            if (bots?.Any() == true)
            {
                return bots.Count();
            }

            return 0;
        });

        public async Task<IList<GenerateAccountResult>> GenerateUsers(string[] names, bool isBot)
        {
            var results = new List<GenerateAccountResult>();
            
            var latestUserInfoUID = await GetLatestUserInfoUID();

            foreach (var name in names)
            {
                latestUserInfoUID += 1;
                var userName = $"{name}{latestUserInfoUID}";
                var password = PasswordHelper.Random();
                var avatarId = new Random().Next(1, 40);
                var result = new GenerateAccountResult(userName, name, password);

                if (string.IsNullOrWhiteSpace(name) || name.Length < 3 || name.Length > 14 || !Regex.IsMatch(name, @"^[a-zA-Z0-9]+$"))
                {
                    result.Error = $"Invalid name {name}";
                    results.Add(result);
                    continue;
                }

                try
                {
                    var createdAccountId = await CreateAccount(userName, password);

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

        private async Task<string> CreateAccount(string username, string password) 
        {
            var hashPassword = PasswordHelper.HashPassword(password);

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

        public async Task<int> ReviseBotNames()
        {
            var bots = accountInfoRepository.FilterBy(entity => entity.isBot && entity.name.Length > 14).ToList();

            foreach (var bot in bots)
            {
                bot.name = bot.name.Substring(0, 13);

                await accountInfoRepository.ReplaceOneAsync(bot);
            }

            return bots.Count;
        }      
    }
}
