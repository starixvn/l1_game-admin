using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Extensions;
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
    }

    public class BotService : IBotService
    {
        private const int DisabledBotRatio = 1;

        private const int minInitAmount = 1000;
        private const int maxInitAmount = 8000;
        private const int minAmount = 500;
        private const int maxAmount = 12000;
        private const int minAddAmount = 1000;
        private const int maxAddAmount = 8000;        

        private readonly IGenericRepository<AccountInfoEntity> accountInfoRepository;
        private readonly ISentryClient sentryClient;

        public BotService(
            IGenericRepository<AccountInfoEntity> accountInfoRepository,
            ISentryClient sentryClient)
        {
            this.accountInfoRepository = accountInfoRepository;
            this.sentryClient = sentryClient;
        }

        public async Task<int> ReviseBalance()
        {
            var total = 0;
            sentryClient.CaptureMessage($"Start task ReviseBalance {DateTime.Now} with minInitAmount {minInitAmount}");

            total += await AddBalance();
            total += await ResetBalance();

            return total;
        }

        public async Task<int> AddBalance()
        {
            var botList = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot && entity.Red < minAmount)).ToList();

            foreach (var bot in botList)
            {
                bot.Red += new Random().Next(minAddAmount, maxAddAmount);

                await accountInfoRepository.ReplaceOneAsync(bot);
            }

            sentryClient.CaptureMessage($"AddBalance for {botList.Count}");

            return botList.Count;
        }

        public async Task<int> ResetBalance()
        {
            const int maxLogs = 100;
            var index = 0;
            var resetMessage = string.Empty;
            var botList = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot && entity.Red >= maxAmount)).ToList();

            sentryClient.CaptureMessage($"ResetBalance for {botList?.Count}");

            foreach (var bot in botList)
            {
                index++;
                bot.Red = new Random().Next(minInitAmount, maxInitAmount);
                await accountInfoRepository.ReplaceOneAsync(bot);

                resetMessage += index < maxLogs ? $" {bot.Name} : new balance {bot.Red};" : string.Empty;
            }

            sentryClient.CaptureMessage(resetMessage);

            return botList.Count;
        }

        public async Task<int> DisableRandomly()
        {
            var total = 0;
            var removedBots = string.Empty;
            var message = string.Empty;

            var disabledBots = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot && entity.Disabled)).ToList();
            var enabledBots = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot && !entity.Disabled)).ToList();
            enabledBots.Shuffle();

            var maxRemoval = DisabledBotRatio * enabledBots.Count / 100;

            message = $"Disabled {maxRemoval} bots.";

            for (var i = 0; i < maxRemoval; i++)
            {
                removedBots += " " + enabledBots[i]?.Name + ";";

                await ToogleEnableDisable(enabledBots[i], true);
            }

            sentryClient.CaptureMessage(message);

            total = maxRemoval;
            total += await Enable(disabledBots);

            return total;
        }

        private async Task<int> Enable(List<AccountInfoEntity> disabledBots)
        {
            var message = $"Enable {disabledBots.Count} bots.";

            foreach (var bot in disabledBots)
            {
                message += " " + bot.Name + ";";
                await ToogleEnableDisable(bot, false);
            }

            sentryClient.CaptureMessage(message);

            return disabledBots.Count;
        }

        private async Task ToogleEnableDisable(AccountInfoEntity bot, bool disable)
        {
            var update = Builders<AccountInfoEntity>.Update.Set(nameof(AccountInfoEntity.Disabled), disable);
            await accountInfoRepository.UpdateAsync(entity => entity.Id == bot.Id, update);
        }
    }
}
