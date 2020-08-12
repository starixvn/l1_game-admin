using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.GameUser.Requests;
using GamesAdmin.Api.GameUser.Results;
using GamesAdmin.Api.GameUsers.Results;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.MiniSedie;
using GamesAdmin.Database.Entities.Spins;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GamesAdmin.Api.GameUser
{
    public interface IGameUserService
    {
        Task<int> GetTotalBots();

        Task<IList<GenerateAccountResult>> GenerateUsers(string[] names, bool isBot);

        Task<int> ReviseBotNames();

        Task<GameAccount> GetAccountAsync(string userId, string nickname, bool excludeUUS);

        Task<AddSpinResult> AddSpin(AddSpinRequest createRequest);
    }

    public class GameUserService : IGameUserService
    {
        private readonly ISentryClient sentryClient;

        private readonly IGenericRepository<AccountInfoEntity> accountInfoRepository;
        private readonly IGenericRepository<AccountEntity> accountRepository;
        private readonly IGenericRepository<SpinEntity> spinRepository;

        private readonly int minInitAmount;
        private readonly int maxInitAmount;

        public GameUserService(
            ISentryClient sentryClient,
            IBotSettings botSettings,
            IGenericRepository<AccountInfoEntity> accountInfoRepository,
            IGenericRepository<AccountEntity> accountRepository)
            IGenericRepository<SpinEntity> spinRepository)
        {
            this.sentryClient = sentryClient;
            this.accountInfoRepository = accountInfoRepository;
            this.accountRepository = accountRepository;
            this.spinRepository = spinRepository;

            minInitAmount = 2 * botSettings.DefaultMaxBet;
            maxInitAmount = 20 * botSettings.DefaultMaxBet;
        }

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

            foreach (var bot in bots)
            {
                bot.InRoom = false;
                bot.Disabled = false;
                bot.InGameRoom = string.Empty;

                await accountInfoRepository.ReplaceOneAsync(bot);
            }
        }

        public async Task<GameAccount> GetAccountAsync(string userId, string nickname, bool excludeUUS)
        {
            var nickNameRegex = new Regex($"(?i){nickname}");

            var account = await accountRepository.FindByIdAsync(userId);
            var accountInfo = await accountInfoRepository
                .FindOneAsync(x => x.UserId == account.Id.ToString()
                    && (!excludeUUS || !(x.Currency == "UUS"))
                    && (string.IsNullOrWhiteSpace(nickname) || nickNameRegex.IsMatch(x.Name)));

            return accountInfo == null || account == null
                ? default
                : new GameAccount(
                    account.Local.Username,
                    account.Local.MemberId,
                    account.Local.MemberName,
                    account.Local.SiteId,
                    account.Local.Language,
                    accountInfo.IsBot,
                    accountInfo.Currency,
                    accountInfo.Name,
                    accountInfo.UID
                );
        }

        public async Task<AddSpinResult> AddSpin(AddSpinRequest createRequest)
        {
            var result = new AddSpinResult { Key = createRequest.Key, ObCustId = createRequest.ObCustId };

            try
            {
                var existingSpins = await GetSpinsByKey(createRequest.Key);

                if (existingSpins.Count > 0)
                {
                    result.Error = existingSpins.Count == createRequest.Spins && existingSpins.All(x => x.ObCustId == createRequest.ObCustId)
                        ? string.Empty
                        : "Existing Key with different spin info.";

                    return result;
                }

                await CreateSpins(createRequest);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                result.Error = ex.Message;
            }

            return result;
        }

        private async Task CreateSpins(AddSpinRequest createRequest)
        {
            var spins = new List<SpinEntity>();
            var time = DateTime.UtcNow;

            for (var i = 0; i < createRequest.Spins; i++)
            {
                spins.Add(new SpinEntity
                {
                    Key = createRequest.ObCustId,
                    ObCustId = createRequest.ObCustId,
                    Time = time
                });
            }

            await spinRepository.InsertManyAsync(spins);
        }

        private async Task<IList<SpinEntity>> GetSpinsByKey(string key)
        => (await spinRepository.FilterByAsync(spin => spin.Key == key)).ToList();
    }
}
