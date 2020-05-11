using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Configurations;
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
        private readonly ISentryClient sentryClient;
        private readonly IBotSettings botSettings;

        private readonly IGenericRepository<AccountInfoEntity> accountInfoRepository;
        private readonly IGenericRepository<AccountEntity> accountRepository;
        private readonly IGenericRepository<BigSmallAccountEntity> txAccountRepository;
        private readonly IGenericRepository<BigSmallTurboAccountEntity> txTurboAccountRepository;
        private readonly IGenericRepository<XocDiaAccountEntity> xdAccountRepository;
        private readonly IGenericRepository<BauCuaAccountEntity> bcAccountRepository;
        private readonly IGenericRepository<OddEvenTurboAccountEntity> oddEvenTurboRepository;
        private readonly IGenericRepository<OddEvenAccountEntity> oddevenRepository;
        private readonly IGenericRepository<DragonTigerAccountEntity> dragonTigerRepository;
        private readonly IGenericRepository<KenoProMaxAccountEntity> kenoProMaxRepository;

        private readonly int minInitAmount;
        private readonly int maxInitAmount;
        private readonly int minAmount;
        private readonly int maxAmount;
        private readonly int maxAddAmount;
        private readonly int minAddAmount;

        public AccountService(
            ISentryClient sentryClient,
            IBotSettings botSettings,
            IGenericRepository<AccountInfoEntity> accountInfoRepository,
            IGenericRepository<AccountEntity> accountRepository,
            IGenericRepository<BigSmallAccountEntity> txAccountRepository,
            IGenericRepository<XocDiaAccountEntity> xdAccountRepository,
            IGenericRepository<BauCuaAccountEntity> bcAccountRepository,
            IGenericRepository<OddEvenTurboAccountEntity> oddEvenTurboRepository,
            IGenericRepository<OddEvenAccountEntity> oddevenRepository,
            IGenericRepository<BigSmallTurboAccountEntity> txTurboAccountRepository,
            IGenericRepository<DragonTigerAccountEntity> dragonTigerRepository,
            IGenericRepository<KenoProMaxAccountEntity> kenoProMaxRepository)
        {
            this.sentryClient = sentryClient;
            this.botSettings = botSettings;
            this.accountInfoRepository = accountInfoRepository;
            this.accountRepository = accountRepository;
            this.txAccountRepository = txAccountRepository;
            this.xdAccountRepository = xdAccountRepository;
            this.bcAccountRepository = bcAccountRepository;
            this.oddEvenTurboRepository = oddEvenTurboRepository;
            this.oddevenRepository = oddevenRepository;
            this.txTurboAccountRepository = txTurboAccountRepository;
            this.dragonTigerRepository = dragonTigerRepository;
            this.kenoProMaxRepository = kenoProMaxRepository;

            minInitAmount = 2 * botSettings.DefaultMaxBet;
            maxInitAmount = 20 * botSettings.DefaultMaxBet;
            minAmount = 2 * botSettings.DefaultMaxBet;
            maxAmount = 20 * botSettings.DefaultMaxBet;
            maxAddAmount = 10 * botSettings.DefaultMaxBet;
            minAddAmount = 3 * botSettings.DefaultMaxBet;
        }

        public Task ReviseBotBalance()
        => Task.Run(() =>
        {

            var botsNeedAddBalance = accountInfoRepository.FilterBy(entity => entity.IsBot && entity.Red < minAmount).ToList();
            var botsNeedResetBalance = accountInfoRepository.FilterBy(entity => entity.IsBot && entity.Red >= maxAmount).ToList();

            sentryClient.CaptureMessage($"AddBalance for {botsNeedAddBalance?.Count}");
            var resetMessage = $"ResetBalance for {botsNeedResetBalance?.Count}";

            foreach (var bot in botsNeedAddBalance)
            {
                bot.Red += new Random().Next(minAddAmount, maxAddAmount);

                accountInfoRepository.ReplaceOne(bot);
            }

            foreach (var bot in botsNeedResetBalance)
            {
                resetMessage += $" {bot.Name} : {bot.Red};";

                bot.Red = new Random().Next(minInitAmount, maxInitAmount);

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
            var defaultHash = BCrypt.Net.BCrypt.HashPassword("bot1234AA", BCrypt.Net.BCrypt.GenerateSalt(12));

            foreach (var name in names)
            {
                latestUserInfoUID += 1;
                var userName = $"{name}{latestUserInfoUID}";
                var password = isBot ? string.Empty : $"pwd{(new Random()).Next(200, 1000)}{new Random().Next(150, 500)}";
                var amount = (new Random()).Next(minInitAmount, maxInitAmount);
                var avatarId = new Random().Next(1, 40);
                var result = new GenerateAccountResult(userName, name, password);

                if (InvalidName(name))
                {
                    result.Error = $"Invalid name {name}";
                    results.Add(result);
                    continue;
                }

                try
                {
                    var hashPassword = isBot
                        ? defaultHash
                        : BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));

                    var account = GenerateAccount(userName, hashPassword);
                    var accountInfo = GenerateAccountInfo(isBot, latestUserInfoUID, name, amount, avatarId);

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
            await JoinToGameCollections(newAccounts);

            return results.ToArray();
        }

        private static bool InvalidName(string name)
        => string.IsNullOrWhiteSpace(name) || name.Length < 3 || name.Length > 14 || !Regex.IsMatch(name, @"^[a-zA-Z0-9]+$");


        private static AccountEntity GenerateAccount(string userName, string hashPassword)
        => new AccountEntity
        {
            Local = new LocalAccount
            {
                Username = userName,
                Password = hashPassword,
                regDate = DateTime.UtcNow
            }
        };


        private static AccountInfoEntity GenerateAccountInfo(bool isBot, int latestUserInfoUID, string name, int amount, int avatarId)
        => new AccountInfoEntity
        {
            Name = name,
            AvatarId = avatarId,
            Red = amount,
            IsBot = isBot,
            JoinedOn = DateTime.UtcNow,
            UID = latestUserInfoUID
        };

        private async Task JoinToGameCollections(List<AccountEntity> newAccounts)
        {
            await txAccountRepository.InsertManyAsync(newAccounts.Select(x => new BigSmallAccountEntity { Uid = x.Id.ToString() }).ToList());
            await xdAccountRepository.InsertManyAsync(newAccounts.Select(x => new XocDiaAccountEntity { Uid = x.Id.ToString() }).ToList());
            await bcAccountRepository.InsertManyAsync(newAccounts.Select(x => new BauCuaAccountEntity { Uid = x.Id.ToString() }).ToList());
            await txTurboAccountRepository.InsertManyAsync(newAccounts.Select(x => new BigSmallTurboAccountEntity { Uid = x.Id.ToString() }).ToList());
            await oddevenRepository.InsertManyAsync(newAccounts.Select(x => new OddEvenAccountEntity { Uid = x.Id.ToString() }).ToList());
            await oddEvenTurboRepository.InsertManyAsync(newAccounts.Select(x => new OddEvenTurboAccountEntity { Uid = x.Id.ToString() }).ToList());
            await dragonTigerRepository.InsertManyAsync(newAccounts.Select(x => new DragonTigerAccountEntity { Uid = x.Id.ToString() }).ToList());
            await kenoProMaxRepository.InsertManyAsync(newAccounts.Select(x => new KenoProMaxAccountEntity { Uid = x.Id.ToString() }).ToList());
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
                await kenoProMaxRepository.InsertManyAsync(bots.Select(x => new KenoProMaxAccountEntity { Uid = x.UserId }).ToList());
            }
            catch (Exception e)
            {
                sentryClient.CaptureException(e);
            }
        }
    }
}
