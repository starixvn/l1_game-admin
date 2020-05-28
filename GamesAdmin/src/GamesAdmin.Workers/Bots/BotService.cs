using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Workers._Shared.Configurations;
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
        private readonly int minInitAmount;
        private readonly int maxInitAmount;
        private readonly int minAmount;
        private readonly int maxAmount;
        private readonly int maxAddAmount;
        private readonly int minAddAmount;

        private readonly IGenericRepository<AccountInfoEntity> accountInfoRepository;
        private readonly ISentryClient sentryClient;


        public BotService(
            IGenericRepository<AccountInfoEntity> accountInfoRepository,
            IBotSettings botSettings,
            ISentryClient sentryClient)
        {
            minInitAmount = 2 * botSettings.DefaultMaxBet;
            maxInitAmount = 20 * botSettings.DefaultMaxBet;
            minAmount = 2 * botSettings.DefaultMaxBet;
            maxAmount = 20 * botSettings.DefaultMaxBet;
            maxAddAmount = 10 * botSettings.DefaultMaxBet;
            minAddAmount = 3 * botSettings.DefaultMaxBet;

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
            var botList = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot && entity.Red >= maxAmount)).ToList();

            var resetMessage = $"ResetBalance for {botList?.Count}";

            foreach (var bot in botList)
            {
                resetMessage += $" {bot.Name} : {bot.Red};";

                bot.Red = new Random().Next(minInitAmount, maxInitAmount);

                await accountInfoRepository.ReplaceOneAsync(bot);
            }

            sentryClient.CaptureMessage(resetMessage);
        }

        public async Task DisableRandomly()
        {
            var disabledBots = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot && entity.Disabled)).ToList();
            var enabledBots = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot && !entity.Disabled)).ToList();
            enabledBots.Shuffle();
           
            var maxRemoval = new Random().Next(5, 15) * enabledBots.Count / 100;

            sentryClient.CaptureMessage($"Disabled {maxRemoval} bots");

            for (var i = 0; i < maxRemoval; i++)
            {
                await ToogleEnableDisable(enabledBots[i], true);
            }

            await Enable(disabledBots);
        }

        private async Task Enable(List<AccountInfoEntity> disabledBots)
        {
            sentryClient.CaptureMessage($"Enable {disabledBots.Count} bots");

            foreach (var bot in disabledBots)
            {
                await ToogleEnableDisable(bot, false);
            }
        }

        private async Task ToogleEnableDisable(AccountInfoEntity bot, bool disable)
        {
            var update = Builders<AccountInfoEntity>.Update.Set(nameof(AccountInfoEntity.Disabled), disable);
            await accountInfoRepository.UpdateAsync(entity => entity.Id == bot.Id, update);
        }
    }
}
