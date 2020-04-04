using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamesAdmin.Api.Users.Results;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Sentry;

namespace GamesAdmin.Api.Users
{
    public interface IUserService
    {
        Task AddBotBalance();

        Task<int> GetTotalBots();

        Task<IList<GenerateUserResult>> GenerateUsers(string[] names, bool isBot);

        Task<int> ReviseBotNames();
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
            var bots = accountInfoRepository.FilterBy(entity => entity.isBot);

            if (bots?.Any() == true)
            {
                return bots.Count();
            }

            return 0;
        });

        public async Task<IList<GenerateUserResult>> GenerateUsers(string[] names, bool isBot)
        {
            var results = new List<GenerateUserResult>();
            var latestUserInfoUID = await GetLatestUserInfoUID();
            var newAccounts = new List<AccountEntity>();
            var newAccountInfos = new List<AccountInfoEntity>();
            var tuples = new List<Tuple<AccountEntity, AccountInfoEntity>>();

            foreach (var name in names)
            {
                latestUserInfoUID += 1;
                var userName = $"{name}{latestUserInfoUID}";
                var password = $"pwd{(new Random()).Next(200, 1000)}{new Random().Next(150, 500)}";
                var avatarId = new Random().Next(1, 40);
                var result = new GenerateUserResult(userName, name, password);

                if (string.IsNullOrWhiteSpace(name) || name.Length < 3 || name.Length > 14 || !Regex.IsMatch(name, @"^[a-zA-Z0-9]+$"))
                {
                    result.Error = $"Invalid name {name}";
                    results.Add(result);
                    continue;
                }

                try
                {
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

                    var accountInfo = new AccountInfoEntity
                    {
                        name = name,
                        avatarId = avatarId,
                        red = 10000,
                        isBot = isBot,
                        joinedOn = DateTime.UtcNow,
                        UID = latestUserInfoUID
                    };

                    newAccounts.Add(account);
                    newAccountInfos.Add(accountInfo);
                    tuples.Add(new Tuple<AccountEntity, AccountInfoEntity>(account, accountInfo));
                }
                catch (Exception ex)
                {
                    sentryClient.CaptureException(ex);
                    result.Error = ex.Message;
                }

                results.Add(result);
            }

            await accountRepository.InsertManyAsync(newAccounts);

            foreach (var tuple in tuples)
            {
                tuple.Item2.userId = tuple.Item1.id.ToString();
            }

            await accountInfoRepository.InsertManyAsync(newAccountInfos);
            await txAccountRepository.InsertManyAsync(newAccounts.Select(x => new MiniSicboAccountEntity { uid = x.id.ToString() }).ToList());
            await xdAccountRepository.InsertManyAsync(newAccounts.Select(x => new XocDiaAccountEntity { uid = x.id.ToString() }).ToList());
            await bcAccountRepository.InsertManyAsync(newAccounts.Select(x => new BauCuaAccountEntity { uid = x.id.ToString() }).ToList());

            return results.ToArray();
        }

        private Task<int> GetLatestUserInfoUID()
          => Task.Run(() => { return accountInfoRepository.FilterBy(userInfo => userInfo.UID > 0).LastOrDefault()?.UID ?? 0; });

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