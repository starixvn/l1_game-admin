using System;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Workers._Shared.Configurations;

namespace GamesAdmin.Workers.Bots
{
    public interface IBotService
    {
        Task ReviseBalance();
    }

    public class BotService : IBotService
    {
        private readonly IGenericRepository<AccountInfoEntity> accountInfoRepository;

        private readonly int minInitAmount;
        private readonly int maxInitAmount;
        private readonly int minAmount;
        private readonly int maxAmount;
        private readonly int maxAddAmount;
        private readonly int minAddAmount;

        public BotService(
            IGenericRepository<AccountInfoEntity> accountInfoRepository,
            IBotSettings botSettings)
        {
            this.accountInfoRepository = accountInfoRepository;

            minInitAmount = 2 * botSettings.DefaultMaxBet;
            maxInitAmount = 20 * botSettings.DefaultMaxBet;
            minAmount = 2 * botSettings.DefaultMaxBet;
            maxAmount = 20 * botSettings.DefaultMaxBet;
            maxAddAmount = 10 * botSettings.DefaultMaxBet;
            minAddAmount = 3 * botSettings.DefaultMaxBet;
        }

        public async Task ReviseBalance()
        {
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
        }

        public async Task ResetBalance()
        {
            var botList = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot && entity.Red >= maxAmount)).ToList();

            //sentryClient.CaptureMessage($"AddBalance for {botsNeedAddBalance?.Count}");
            var resetMessage = $"ResetBalance for {botList?.Count}";

            foreach (var bot in botList)
            {
                resetMessage += $" {bot.Name} : {bot.Red};";

                bot.Red = new Random().Next(minInitAmount, maxInitAmount);

                await accountInfoRepository.ReplaceOneAsync(bot);
            }

            //sentryClient.CaptureMessage(resetMessage);
        }
    }
}
