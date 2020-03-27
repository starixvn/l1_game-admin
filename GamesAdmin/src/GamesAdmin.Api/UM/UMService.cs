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
        Task<bool> AddSchedule(DateTime start, DateTime end);

        Task<bool> Process(bool isUM);
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

        public async Task<bool> AddSchedule(DateTime start, DateTime end)
        {
            try
            {
                await umRepository.InsertOneAsync(new UMInfoEntity
                {
                    start = start,
                    end = end,
                    finish = false,
                    time = DateTime.UtcNow
                }); ;

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public Task<bool> Process(bool isUM)=> isUM ? Start() : End();

        public async Task<bool> Start()
        {
            try
            {
                await UpdateEnableConfig(false);

                await umRepository.InsertOneAsync(new UMInfoEntity
                {
                    start = DateTime.UtcNow,
                    end = default,
                    finish = false,
                    time = DateTime.UtcNow
                }); ;

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
                    umInfo.end = DateTime.UtcNow;
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
