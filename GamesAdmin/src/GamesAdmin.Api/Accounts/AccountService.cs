using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamesAdmin.Api.Accounts.Results;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Sentry;

namespace GamesAdmin.Api.Accounts
{
    public interface IAccountService
    {
        Task ReviseBotBalance();

        Task<int> GetTotalBots();

        Task<IList<GenerateAccountResult>> GenerateUsers(string[] names, bool isBot);

        Task<int> ReviseBotNames();
    }

    public class AccountService : IAccountService
    {
        private const int MinAmount = 500;
        private const int MaxAmount = 10000;

        private const int MinInitAmount = 700;
        private const int MaxInitAmount = 2000;

        private const int MinAddAmount = 1000;
        private const int MaxAddAmount = 2000;

        private readonly IGenericRepository<AccountInfoEntity> accountInfoRepository;
        private readonly IGenericRepository<AccountEntity> accountRepository;
        private readonly IGenericRepository<BigSmallAccountEntity> txAccountRepository;
        private readonly IGenericRepository<XocDiaAccountEntity> xdAccountRepository;
        private readonly IGenericRepository<BauCuaAccountEntity> bcAccountRepository;
        private readonly IGenericRepository<OddEvenTurboAccountEntity> oddEvenTurboRepository;
        private readonly IGenericRepository<OddEvenAccountEntity> oddevenRepository;

        private readonly ISentryClient sentryClient;

        public AccountService(
            ISentryClient sentryClient,
            IGenericRepository<AccountInfoEntity> accountInfoRepository,
            IGenericRepository<AccountEntity> accountRepository,
            IGenericRepository<BigSmallAccountEntity> txAccountRepository,
            IGenericRepository<XocDiaAccountEntity> xdAccountRepository,
            IGenericRepository<BauCuaAccountEntity> bcAccountRepository,
            IGenericRepository<OddEvenTurboAccountEntity> oddEvenTurboRepository,
            IGenericRepository<OddEvenAccountEntity> oddevenRepository)
        {
            this.sentryClient = sentryClient;
            this.accountInfoRepository = accountInfoRepository;
            this.accountRepository = accountRepository;
            this.txAccountRepository = txAccountRepository;
            this.xdAccountRepository = xdAccountRepository;
            this.bcAccountRepository = bcAccountRepository;
            this.oddEvenTurboRepository = oddEvenTurboRepository;
            this.oddevenRepository = oddevenRepository;
        }

        public Task ReviseBotBalance()
        => Task.Run(() =>
        {
           
            var botsNeedAddBalance = accountInfoRepository.FilterBy(entity => entity.IsBot && entity.Red < MinAmount).ToList();           
            var botsNeedResetBalance = accountInfoRepository.FilterBy(entity => entity.IsBot && entity.Red >= MaxAmount).ToList();

            sentryClient.CaptureMessage($"AddBalance for {botsNeedAddBalance?.Count}");
            var resetMessage = $"ResetBalance for {botsNeedResetBalance?.Count}";

            foreach (var bot in botsNeedAddBalance)
            {
                bot.Red += new Random().Next(MinAddAmount, MaxAddAmount);

                accountInfoRepository.ReplaceOne(bot);
            }

            foreach (var bot in botsNeedResetBalance)
            {
                resetMessage += $" {bot.Name} : {bot.Red};";

                bot.Red = new Random().Next(MinInitAmount, MaxInitAmount);

                accountInfoRepository.ReplaceOne(bot);
            }

            sentryClient.CaptureMessage(resetMessage);
        });

        public async Task<int> GetTotalBots()      
        {
            var bots = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot)).ToList();

            if (bots?.Any() == true)
            {
                return bots.Count();
            }

            return 0;
        }

        public async Task<IList<GenerateAccountResult>> GenerateUsers(string[] names, bool isBot)
        {
            var results = new List<GenerateAccountResult>();


            var latestUserInfoUID = await GetLatestUserInfoUID();
            var newAccounts = new List<AccountEntity>();
            var newAccountInfos = new List<AccountInfoEntity>();
            var tuples = new List<Tuple<AccountEntity, AccountInfoEntity>>();

            foreach (var name in names)
            {
                latestUserInfoUID += 1;
                var userName = $"{name}{latestUserInfoUID}";
                var password = $"pwd{(new Random()).Next(200, 1000)}{new Random().Next(150, 500)}";
                var amount = (new Random()).Next(MinInitAmount, MaxInitAmount);
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
                    var hashPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));

                    var account = new AccountEntity
                    {
                        Local = new LocalAccount
                        {
                            Username = userName,
                            Password = hashPassword,
                            regDate = DateTime.UtcNow
                        }
                    };

                    var accountInfo = new AccountInfoEntity
                    {
                        Name = name,
                        AvatarId = avatarId,
                        Red = amount,
                        IsBot = isBot,
                        JoinedOn = DateTime.UtcNow,
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
                tuple.Item2.UserId = tuple.Item1.Id.ToString();
            }

            await accountInfoRepository.InsertManyAsync(newAccountInfos);
            await txAccountRepository.InsertManyAsync(newAccounts.Select(x => new BigSmallAccountEntity { Uid = x.Id.ToString() }).ToList());
            await xdAccountRepository.InsertManyAsync(newAccounts.Select(x => new XocDiaAccountEntity { Uid = x.Id.ToString() }).ToList());
            await bcAccountRepository.InsertManyAsync(newAccounts.Select(x => new BauCuaAccountEntity { Uid = x.Id.ToString() }).ToList());

            return results.ToArray();
        }

        private Task<int> GetLatestUserInfoUID()
            => Task.Run(() => { return accountInfoRepository.FilterBy(userInfo => userInfo.UID > 0).LastOrDefault()?.UID ?? 0; });

        public async Task<int> ReviseBotNames()
        {
            var bots = accountInfoRepository
                .FilterBy(entity => entity.IsBot && (entity.Name.Length > 14))
                .ToList();

            foreach (var bot in bots)
            {
                bot.Name = bot.Name.Substring(0, 13);

                await accountInfoRepository.ReplaceOneAsync(bot);
            }

            await MigrateBots();

            return bots.Count;
        }

        private async Task MigrateBots() 
        {
            var bots = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot)).ToList();

            try
            {
                await oddEvenTurboRepository.InsertManyAsync(bots.Select(x => new OddEvenTurboAccountEntity { Uid = x.UID.ToString() }).ToList());
            }
            catch (Exception e) 
            {
                sentryClient.CaptureException(e);
            }

            try
            {
                await oddevenRepository.InsertManyAsync(bots.Select(x => new OddEvenAccountEntity { Uid = x.UID.ToString() }).ToList());
            }
            catch (Exception e)
            {
                sentryClient.CaptureException(e);
            }
        }
    }
}
