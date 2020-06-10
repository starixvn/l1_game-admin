using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Workers.Extensions;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Workers.Bots
{
    public interface IBotService
    {
        Task ReviseBalance();

        Task DisableRandomly();
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

        public async Task ReviseBalance()
        {
            sentryClient.CaptureMessage($"Start task ReviseBalance {DateTime.Now} with minInitAmount {minInitAmount}");

            await AddBalance();
            await ResetBalance();
        }

        public async Task AddBalance()
        {
            var botList = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot && entity.Red < minAmount)).ToList();

            foreach (var bot in botList)
            {
                bot.Red += new Random().Next(minAddAmount, maxAddAmount);

                await accountInfoRepository.ReplaceOneAsync(bot);
            }

            sentryClient.CaptureMessage($"AddBalance for {botList?.Count}");
        }

        public async Task ResetBalance()
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
        }

        public async Task DisableRandomly()
        {
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

            await Enable(disabledBots);
        }

        private async Task Enable(List<AccountInfoEntity> disabledBots)
        {
            var message = $"Enable {disabledBots.Count} bots.";

            foreach (var bot in disabledBots)
            {
                message += " " + bot.Name + ";";
                await ToogleEnableDisable(bot, false);
            }

            sentryClient.CaptureMessage(message);
        }

        private async Task ToogleEnableDisable(AccountInfoEntity bot, bool disable)
        {
            var update = Builders<AccountInfoEntity>.Update.Set(nameof(AccountInfoEntity.Disabled), disable);
            await accountInfoRepository.UpdateAsync(entity => entity.Id == bot.Id, update);
        }
    }
}
