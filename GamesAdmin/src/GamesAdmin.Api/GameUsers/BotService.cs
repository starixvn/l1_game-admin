using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Extensions;
using GamesAdmin.Api.GameUser.Results;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.GameUser
{
    public interface IBotService
    {
        Task<int> ReviseBalance();

        Task<int> DisableRandomly();

        Task<int> GetTotal();

        Task<IList<GenerateAccountResult>> Generate(string[] names);

        Task<int> BannedRandomly();
    }

    public class BotService : IBotService
    {
        private const int DisabledBotRatio = 1;
        private const int BannedBotRatio = 10;

        private const int minBalance = 500;
        private const int maxBalance = 3000;
        private const int minAmount = 300;
        private const int maxAmount = 7000;
        private const int minAddAmount = 1000;
        private const int maxAddAmount = 3000;

        private const int minAmountScale = 2;
        private const int maxAmountScale = 20;
        private const int minCharNameLength = 3;
        private const int maxCharNameLength = 14;
        private const int defaultMaxBet = 1000;

        private readonly int minInitAmount = minAmountScale * defaultMaxBet;
        private readonly int maxInitAmount = maxAmountScale * defaultMaxBet;

        private readonly IGenericRepository<AccountInfoEntity> accountInfoRepository;
        private readonly ISentryClient sentryClient;
        private readonly IGenericRepository<AccountEntity> accountRepository;

        public BotService(
            IGenericRepository<AccountInfoEntity> accountInfoRepository,
            ISentryClient sentryClient,
            IGenericRepository<AccountEntity> accountRepository)
        {
            this.accountInfoRepository = accountInfoRepository;
            this.sentryClient = sentryClient;
            this.accountRepository = accountRepository;
        }

        public async Task<int> ReviseBalance()
        {
            sentryClient.CaptureMessage($"Start task ReviseBalance {DateTime.Now} with minInitAmount {minBalance}");

            return await AddBalance() + await ResetBalance();
        }

        public async Task<int> AddBalance()
        {
            var botList = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot && entity.Red < minAmount)).ToList();

            foreach (var bot in botList)
            {               
                var update = Builders<AccountInfoEntity>.Update.Set(nameof(AccountInfoEntity.Red), bot.Red + new Random().Next(minAddAmount, maxAddAmount));
                await accountInfoRepository.UpdateAsync(entity => entity.Id == bot.Id, update);
            }

            sentryClient.CaptureMessage($"AddBalance for {botList.Count}");

            return botList.Count;
        }

        public async Task<int> ResetBalance()
        {
            var botList = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot && entity.Red >= maxAmount)).ToList();
            var strBuilder = new StringBuilder($"ResetBalance for {botList.Count}");

            foreach (var bot in botList)
            {
                var update = Builders<AccountInfoEntity>.Update.Set(nameof(AccountInfoEntity.Red), new Random().Next(minBalance, maxBalance));
                await accountInfoRepository.UpdateAsync(entity => entity.Id == bot.Id, update);

                strBuilder.Append($" {bot.Name} : new balance {bot.Red};");
            }

            sentryClient.CaptureMessage(strBuilder.ToString());

            return botList.Count;
        }

        public async Task<int> DisableRandomly()
        {
            var disabledBots = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot && entity.Disabled)).ToList();
            var enabledBots = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot && !entity.Disabled)).ToList();
            enabledBots.Shuffle();

            var maxRemoval = DisabledBotRatio * enabledBots.Count / 100;
            var strBuilder = new StringBuilder($"Disabled {maxRemoval} bots.");

            for (var i = 0; i < maxRemoval; i++)
            {
                strBuilder.Append($" {enabledBots[i]?.Name};");

                await ToogleEnableDisable(enabledBots[i], true);
            }

            sentryClient.CaptureMessage(strBuilder.ToString());

            return maxRemoval + await Enable(disabledBots);
        }

        private async Task<int> Enable(List<AccountInfoEntity> disabledBots)
        {
            var strBuilder = new StringBuilder($"Enable {disabledBots.Count} bots.");

            foreach (var bot in disabledBots)
            {
                strBuilder.Append($" {bot.Name};");
                await ToogleEnableDisable(bot, false);
            }

            sentryClient.CaptureMessage(strBuilder.ToString());

            return disabledBots.Count;
        }

        private async Task ToogleEnableDisable(AccountInfoEntity bot, bool disable)
        {
            var update = Builders<AccountInfoEntity>.Update.Set(nameof(AccountInfoEntity.Disabled), disable);
            await accountInfoRepository.UpdateAsync(entity => entity.Id == bot.Id, update);
        }

        public async Task<int> GetTotal()
        {
            var bots = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot)).ToList();

            return bots.Count;
        }

        public async Task<IList<GenerateAccountResult>> Generate(string[] names)
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
                var password = string.Empty;
                var amount = (new Random()).Next(minInitAmount, maxInitAmount);
                var avatarId = new Random().Next(1, 40);
                var result = new GenerateAccountResult(userName, name, password);

                if (InvalidName(name))
                {
                    result.ErrorDescription = $"Invalid name {name}";
                    results.Add(result);
                    continue;
                }

                try
                {
                    var account = GenerateAccount(userName, defaultHash);
                    var accountInfo = GenerateAccountInfo(latestUserInfoUID, name, amount, avatarId);

                    newAccounts.Add(account);
                    newAccountInfos.Add(accountInfo);
                    tuples.Add(new Tuple<AccountEntity, AccountInfoEntity>(account, accountInfo));
                }
                catch (Exception ex)
                {
                    sentryClient.CaptureException(ex);
                    result.ErrorDescription = ex.Message;
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
        => string.IsNullOrWhiteSpace(name) || name.Length < minCharNameLength || name.Length > maxCharNameLength || !Regex.IsMatch(name, @"^[a-zA-Z0-9]+$");

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

        private static AccountInfoEntity GenerateAccountInfo(int latestUserInfoUID, string name, int amount, int avatarId)
        => new AccountInfoEntity
        {
            Name = name,
            AvatarId = avatarId,
            Red = amount,
            IsBot = true,
            JoinedOn = DateTime.UtcNow,
            UID = latestUserInfoUID
        };

        private Task<int> GetLatestUserInfoUID()
            => Task.Run(() => { return accountInfoRepository.FilterBy(userInfo => userInfo.UID > 0).LastOrDefault()?.UID ?? 0; });

        public async Task<int> BannedRandomly()
        {
            var bannedBots = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot && entity.Banned)).ToList();
            var unbannedBots = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot && !entity.Banned)).ToList();
            bannedBots.Shuffle();

            var maxRemoval = BannedBotRatio * unbannedBots.Count / 100;
            var strBuilder = new StringBuilder($"Banned {maxRemoval} bots.");

            for (var i = 0; i < maxRemoval; i++)
            {
                strBuilder.Append($" {bannedBots[i]?.Name};");

                await ToogleBanned(bannedBots[i], true);
            }

            sentryClient.CaptureMessage(strBuilder.ToString());

            foreach (var bot in bannedBots)
            {
                await ToogleBanned(bot, true);
            }           

            return maxRemoval;
        }

        private async Task ToogleBanned(AccountInfoEntity bot, bool banned)
        {
            var update = Builders<AccountInfoEntity>.Update.Set(nameof(AccountInfoEntity.Banned), banned);
            await accountInfoRepository.UpdateAsync(entity => entity.Id == bot.Id, update);
        }
    }
}
