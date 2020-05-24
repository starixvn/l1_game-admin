using System;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

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

        public BotService(IGenericRepository<AccountInfoEntity> accountInfoRepository)
        {
            this.accountInfoRepository = accountInfoRepository;
        }

        public async Task ReviseBalance()
        {
            var botsNeedAddBalance = (await accountInfoRepository.FilterByAsync(entity => entity.IsBot && entity.Red < minAmount)).ToList();
            var botsNeedResetBalance =( await accountInfoRepository.FilterByAsync(entity => entity.IsBot && entity.Red >= maxAmount)).ToList();

            //sentryClient.CaptureMessage($"AddBalance for {botsNeedAddBalance?.Count}");
            var resetMessage = $"ResetBalance for {botsNeedResetBalance?.Count}";

            foreach (var bot in botsNeedAddBalance)
            {
                bot.Red += new Random().Next(minAddAmount, maxAddAmount);

                await accountInfoRepository.ReplaceOneAsync(bot);
            }

            foreach (var bot in botsNeedResetBalance)
            {
                resetMessage += $" {bot.Name} : {bot.Red};";

                bot.Red = new Random().Next(minInitAmount, maxInitAmount);

                await accountInfoRepository.ReplaceOneAsync(bot);
            }

            //sentryClient.CaptureMessage(resetMessage);
        }
    }
}
