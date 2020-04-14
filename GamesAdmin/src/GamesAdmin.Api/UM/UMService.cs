using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Sentry;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.UM
{
    public interface IUMService
    {
        Task<bool> Process(bool isUM, DateTimeOffset startTime, DateTimeOffset endTime);
    }

    public class UMService : IUMService
    {
        private readonly ISentryClient sentryClient;
        private readonly IGenericRepository<UMInfoEntity> umRepository;
        private readonly IGenericRepository<GameConfigEntity> configRepository;

        public UMService(
            ISentryClient sentryClient,
            IGenericRepository<UMInfoEntity> umRepository,
            IGenericRepository<GameConfigEntity> configRepository)
        {
            this.sentryClient = sentryClient;
            this.umRepository = umRepository;
            this.configRepository = configRepository;
        }

        public Task<bool> Process(bool isUM, DateTimeOffset startTime, DateTimeOffset endTime) => isUM ? Start(startTime, endTime) : End();

        public async Task<bool> Start(DateTimeOffset startTime, DateTimeOffset endTime)
        {
            try
            {
                await UpdateEnableConfig(false);

                var umInfo = umRepository.FilterBy(um => !um.finish).FirstOrDefault();

                if (umInfo == null)
                {
                    await umRepository.InsertOneAsync(new UMInfoEntity
                    {
                        start = startTime == default ?  default : startTime.ToUniversalTime().UtcDateTime,
                        end = endTime == default ? default : endTime.ToUniversalTime().UtcDateTime,
                        finish = false,
                        time = DateTime.UtcNow
                    });
                }

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> End()
        {
            try
            {
                await UpdateEnableConfig(true);

                var umInfo = umRepository.FilterBy(um => !um.finish).FirstOrDefault();

                if (umInfo != null)
                {
                    umInfo.finish = true;

                    await umRepository.ReplaceOneAsync(umInfo);
                }

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        private async Task UpdateEnableConfig(bool enable)
        {
            var configs = configRepository.AsQueryable().ToList();

            foreach (var game in configs)
            {
                game.enabled = enable;

                await configRepository.ReplaceOneAsync(game);
            }
        }
    }
}
